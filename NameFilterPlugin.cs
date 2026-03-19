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

        internal static Harmony Harmony = new Harmony(PluginGuid);
        internal static BepInEx.Logging.ManualLogSource? Logger;

        internal static Dictionary<int, string> PreviousNames = new Dictionary<int, string>();

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
                }
            }
        }

        // Warns host when a player changes to a banned name in lobby
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetName))]
        public static class PlayerNameChangePatch
        {
            public static void Prefix(PlayerControl __instance, string name)
            {
                if (!PreviousNames.ContainsKey(__instance.OwnerId))
                    PreviousNames[__instance.OwnerId] = __instance.Data.PlayerName;
            }

            public static void Postfix(PlayerControl __instance, string name)
            {
                if (!AmongUsClient.Instance.AmHost) return;
                if (__instance.AmOwner) return;

                string oldName = PreviousNames.ContainsKey(__instance.OwnerId)
                    ? PreviousNames[__instance.OwnerId]
                    : "Unknown";

                PreviousNames[__instance.OwnerId] = name;

                if (NameChecker.IsBanned(name, out string id, out var severity))
                {
                    string label = NameChecker.GetChatLabel(id, severity);
                    string formattedLabel = FormatLabel(label);

                    Logger?.LogInfo($"[NameFilter] Player \"{oldName}\" changed name to \"{name}\" {label}");

                    string warnMsg = DevMode
                        ? $"<color=#FF0000>{oldName}</color> {Capitalize("changed")} their name to \"{name}\" {formattedLabel} which is banned."
                        : $"<color=#FF0000>{oldName}</color> {Capitalize("changed")} their name to {formattedLabel} which is banned.";

                    MiscUtils.AddFakeChat(
                        PlayerControl.LocalPlayer.Data,
                        "<color=#FF0000>NameFilter Warning</color>",
                        warnMsg,
                        altColors: true
                    );
                }
            }
        }
    }
}
