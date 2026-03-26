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
        public const string PluginVersion = "1.0.0";

        // Set to true to see real banned names in chat (dev only!)
        // Set to false before building public release
        private const bool DevMode = false;

        // Paste your Discord webhook URL here
        private const string DiscordWebhookUrl = "YOUR_WEBHOOK_URL_HERE";

        internal static Harmony Harmony = new Harmony(PluginGuid);
        internal static BepInEx.Logging.ManualLogSource? Logger;
        internal static readonly System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient();

        // Stores previous names per client ID
        internal static Dictionary<int, string> PreviousNames = new Dictionary<int, string>();

        // Stores players who have already been warned about a name change
        internal static HashSet<int> WarnedPlayers = new HashSet<int>();

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
            if (string.IsNullOrEmpty(DiscordWebhookUrl) || DiscordWebhookUrl == "YOUR_WEBHOOK_URL_HERE")
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

        // Kicks players with banned names when joining
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

        // Handles name changes in lobby
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetName))]
        public static class PlayerNameChangePatch
        {
            public static bool Prefix(PlayerControl __instance, string name)
            {
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

                    // Check if player has been warned before
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
                        // First attempt - warn and block name change
                        WarnedPlayers.Add(__instance.OwnerId);

                        MiscUtils.AddFakeChat(
                            PlayerControl.LocalPlayer.Data,
                            "<color=#FF0000>NameFilter Warning</color>",
                            $"<color=#FF0000>{oldName}</color> {Capitalize("tried")} to change their name to {formattedLabel} which is banned.",
                            altColors: true
                        );

                        _ = SendDiscordMessage($"⚠️ [NameFilter] Player \\\"{oldName}\\\" tried to change name to \\\"{name}\\\" {label}");
                    }

                    // Block the name change
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
