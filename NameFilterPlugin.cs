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
        public const string PluginVersion = "1.0.1";

        private const bool DevMode = false;
        private const string DiscordWebhookUrl = "YOUR_WEBHOOK_URL_HERE";

        internal static Harmony Harmony = new Harmony(PluginGuid);
        internal static BepInEx.Logging.ManualLogSource? Logger;
        internal static readonly System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient();

        internal static Dictionary<int, string> PreviousNames = new Dictionary<int, string>();
        internal static HashSet<int> WarnedPlayers = new HashSet<int>();

        // Flag to prevent infinite loop when we reset the name
        internal static bool IsResettingName = false;

        public override void Load()
        {
            Logger = Log;
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
        }

        private static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static string FormatLabel(string label)
        {
            return $"<b><color=#FF0000>{label}</color></b>";
        }

        private static async System.Threading.Tasks.Task SendDiscordMessage(string message)
        {
            if (string.IsNullOrEmpty(DiscordWebhookUrl) || DiscordWebhookUrl == "https://discord.com/api/webhooks/1484261136680620042/yWOmOL3EGjOTYf4Onot6rTHP4Xpw4JkJe8RSRSxccUJ-2FuO9dYYeBR9BNB2MhCh0KOS")
                return;

            try
            {
                var payload = $"{{\"content\": \"{message}\"}}";
                var content = new System.Net.Http.StringContent(payload, System.Text.Encoding.UTF8, "application/json");
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
                    string label = NameChecker.GetChatLabel(id, severity);
                    string formattedLabel = FormatLabel(label);

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

                    _ = SendDiscordMessage($"🚨 [NameFilter] Player \\\"{playerName}\\\" joined and was kicked {label}");
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetName))]
        public static class PlayerNameChangePatch
        {
            public static bool Prefix(PlayerControl __instance, string name)
            {
                // Skip if we're the ones resetting the name
                if (IsResettingName) return true;
                if (!AmongUsClient.Instance.AmHost) return true;
                if (__instance.AmOwner) return true;

                string oldName = PreviousNames.ContainsKey(__instance.OwnerId)
                    ? PreviousNames[__instance.OwnerId]
                    : "Unknown";

                if (NameChecker.IsBanned(name, out string id, out var severity))
                {
                    string label = NameChecker.GetChatLabel(id, severity);
                    string formattedLabel = FormatLabel(label);

                    Logger?.LogInfo($"[NameFilter] Player \"{oldName}\" tried to change name to \"{name}\" {label}");

                    // Reset the name back to old name for everyone including host
                    IsResettingName = true;
                    __instance.RpcSetName(oldName);
                    IsResettingName = false;

                    if (WarnedPlayers.Contains(__instance.OwnerId))
                    {
                        // Ban on second attempt
                        AmongUsClient.Instance.KickPlayer(__instance.OwnerId, true);

                        MiscUtils.AddFakeChat(
                            PlayerControl.LocalPlayer.Data,
                            "<color=#FF0000>NameFilter</color>",
                            $"<color=#FF0000>{oldName}</color> was banned for repeatedly trying to use a banned name. Flagged as {formattedLabel}.",
                            altColors: true
                        );

                        _ = SendDiscordMessage($"🚨 [NameFilter] Player \\\"{oldName}\\\" was banned for repeatedly trying to change to banned name \\\"{name}\\\" {label}");
                    }
                    else
                    {
                        // First attempt - warn
                        WarnedPlayers.Add(__instance.OwnerId);

                        MiscUtils.AddFakeChat(
                            PlayerControl.LocalPlayer.Data,
                            "<color=#FF0000>NameFilter Warning</color>",
                            $"<color=#FF0000>{oldName}</color> {Capitalize("tried")} to change their name to {formattedLabel} which is banned.",
                            altColors: true
                        );

                        _ = SendDiscordMessage($"⚠️ [NameFilter] Player \\\"{oldName}\\\" tried to change name to \\\"{name}\\\" {label}");
                    }

                    return false;
                }

                // Name is clean - update stored name and remove from warned list
                PreviousNames[__instance.OwnerId] = name;
                WarnedPlayers.Remove(__instance.OwnerId);
                return true;
            }
        }
    }
}

