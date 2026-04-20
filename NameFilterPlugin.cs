using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using InnerNet;
using TownOfUs.Utilities;

namespace NameFilter
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Among Us.exe")]
    public class NameFilterPlugin : BasePlugin
    {
        public const string PluginGuid    = "com.namefilter.plugin";
        public const string PluginName    = "NameFilter";
        public const string PluginVersion = "1.0.3";

        private const bool DevMode = false;
        internal const string DiscordWebhookUrl = "https://discord.com/api/webhooks/1484261136680620042/yWOmOL3EGjOTYf4Onot6rTHP4Xpw4JkJe8RSRSxccUJ-2FuO9dYYeBR9BNB2MhCh0KOS";

        internal static Harmony Harmony = new Harmony(PluginGuid);
        internal static BepInEx.Logging.ManualLogSource? Logger;
        internal static readonly System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient();

        internal static Dictionary<int, string> PreviousNames = new Dictionary<int, string>();
        internal static HashSet<int> WarnedPlayers = new HashSet<int>();

        internal static bool IsResettingName = false;

        public override void Load()
        {
            Logger = Log;
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
        }

        internal static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        internal static string FormatLabel(string label)
        {
            return $"<b><color=#FF0000>{label}</color></b>";
        }

        internal static async System.Threading.Tasks.Task SendDiscordEmbed(
            string title,
            int color,
            string playerName,
            string hostName,
            string gameCode,
            string wordId,
            string severity,
            string triggerName,
            string emoji)
        {
            if (string.IsNullOrEmpty(DiscordWebhookUrl) || DiscordWebhookUrl == "YOUR_WEBHOOK_URL_HERE")
                return;

            try
            {
                var payload = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title     = $"{emoji} {title}",
                            color     = color,
                            fields    = new object[]
                            {
                                new { name = "👑 Host",           value = hostName,    inline = true  },
                                new { name = "🎮 Game Code",      value = gameCode,    inline = true  },
                                new { name = "\u200B",            value = "\u200B",    inline = false },
                                new { name = "🚫 Player",         value = playerName,  inline = true  },
                                new { name = "🏷️ Word ID",        value = wordId,      inline = true  },
                                new { name = "⚠️ Severity",       value = severity,    inline = true  },
                                new { name = "💬 Triggered Name", value = $"||{triggerName}||", inline = false },
                            },
                            footer    = new { text = "NameFilter · Among Us Moderation" },
                            timestamp = System.DateTime.UtcNow.ToString("o")
                        }
                    }
                };

                var json    = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await HttpClient.PostAsync(DiscordWebhookUrl, content);
            }
            catch (System.Exception ex)
            {
                Logger?.LogError($"[NameFilter] Failed to send Discord message: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public static class PlayerJoinPatch
        {
            public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ClientData client)
            {
                if (!AmongUsClient.Instance.AmHost) return;
                if (client.Id == AmongUsClient.Instance.ClientId) return;

                string playerName = client.PlayerName;
                PreviousNames[client.Id] = playerName;

                if (NameChecker.IsBanned(playerName, out string id, out var severity))
                {
                    string label          = NameChecker.GetChatLabel(id, severity);
                    string formattedLabel = FormatLabel(label);
                    string hostName       = PlayerControl.LocalPlayer.name;
                    string gameCode       = GameCode.IntToGameName(AmongUsClient.Instance.GameId);

                    Logger?.LogInfo($"[NameFilter] Player \"{playerName}\" joined with banned name {label}");

                    AmongUsClient.Instance.KickPlayer(client.Id, false);

                    string kickMsg = DevMode
                        ? $"A player was kicked. Name: \"{playerName}\" flagged as {formattedLabel}."
                        : $"A player was kicked. Name flagged as {formattedLabel}.";

                    MiscUtils.AddFakeChat(
                        PlayerControl.LocalPlayer.Data,
                        "<color=#FF0000>NameFilter</color>",
                        kickMsg,
                        altColors: true
                    );

                    _ = SendDiscordEmbed(
                        title:       "Player kicked on join",
                        color:       0xFFA500,
                        playerName:  playerName,
                        hostName:    hostName,
                        gameCode:    gameCode,
                        wordId:      id,
                        severity:    severity.ToString(),
                        triggerName: playerName,
                        emoji:       "🚨"
                    );
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetName))]
        public static class PlayerNameChangePatch
        {
            public static bool Prefix(PlayerControl __instance, string name)
            {
                if (IsResettingName) return true;
                if (!AmongUsClient.Instance.AmHost) return true;
                if (__instance.AmOwner) return true;

                string oldName  = PreviousNames.ContainsKey(__instance.OwnerId) ? PreviousNames[__instance.OwnerId] : "Unknown";
                string hostName = PlayerControl.LocalPlayer.name;
                string gameCode = GameCode.IntToGameName(AmongUsClient.Instance.GameId);

                if (NameChecker.IsBanned(name, out string id, out var severity))
                {
                    string label          = NameChecker.GetChatLabel(id, severity);
                    string formattedLabel = FormatLabel(label);

                    Logger?.LogInfo($"[NameFilter] Player \"{oldName}\" tried to change name to \"{name}\" {label}");

                    IsResettingName = true;
                    __instance.RpcSetName(oldName);
                    IsResettingName = false;

                    if (WarnedPlayers.Contains(__instance.OwnerId))
                    {
                        AmongUsClient.Instance.KickPlayer(__instance.OwnerId, true);

                        MiscUtils.AddFakeChat(
                            PlayerControl.LocalPlayer.Data,
                            "<color=#FF0000>NameFilter</color>",
                            $"<color=#FF0000>{oldName}</color> was banned for repeatedly trying to use a banned name. Flagged as {formattedLabel}.",
                            altColors: true
                        );

                        _ = SendDiscordEmbed(
                            title:       "Player banned for repeated violation",
                            color:       0xFF0000,
                            playerName:  oldName,
                            hostName:    hostName,
                            gameCode:    gameCode,
                            wordId:      id,
                            severity:    severity.ToString(),
                            triggerName: name,
                            emoji:       "🔨"
                        );
                    }
                    else
                    {
                        WarnedPlayers.Add(__instance.OwnerId);

                        MiscUtils.AddFakeChat(
                            PlayerControl.LocalPlayer.Data,
                            "<color=#FF0000>NameFilter Warning</color>",
                            $"<color=#FF0000>{oldName}</color> {Capitalize("tried")} to change their name to {formattedLabel} which is banned.",
                            altColors: true
                        );

                        _ = SendDiscordEmbed(
                            title:       "Player warned for banned name",
                            color:       0xFFFF00,
                            playerName:  oldName,
                            hostName:    hostName,
                            gameCode:    gameCode,
                            wordId:      id,
                            severity:    severity.ToString(),
                            triggerName: name,
                            emoji:       "⚠️"
                        );
                    }

                    return false;
                }

                PreviousNames[__instance.OwnerId] = name;
                WarnedPlayers.Remove(__instance.OwnerId);
                return true;
            }
        }
    }
}
