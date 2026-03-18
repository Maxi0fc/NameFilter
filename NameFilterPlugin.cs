using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace NameFilter
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Among Us.exe")]
    public class NameFilterPlugin : BasePlugin
    {
        public const string PluginGuid = "com.namefilter.plugin";
        public const string PluginName = "NameFilter";
        public const string PluginVersion = "1.0.0";

        internal static Harmony Harmony = new Harmony(PluginGuid);
        internal static BepInEx.Logging.ManualLogSource? Logger;

        // Stores previous names per client ID
        internal static Dictionary<int, string> PreviousNames = new Dictionary<int, string>();

        public override void Load()
        {
            Logger = Log;
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
        }

        // Kicks players with banned names when joining
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public static class PlayerJoinPatch
        {
            public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ClientData client)
            {
                if (!AmongUsClient.Instance.AmHost) return;
                if (client.Character == null) return;
                if (client.Character.AmOwner) return;

                string playerName = client.PlayerName;
                PreviousNames[client.Id] = playerName;

                if (NameChecker.IsBanned(playerName, out string matchedWord))
                {
                    Logger?.LogInfo($"[NameFilter] Kicking player with disallowed name: {playerName}");

                    AmongUsClient.Instance.KickPlayer(client.Id, false);

                    HudManager.Instance.Chat.AddChat(
                        PlayerControl.LocalPlayer,
                        $"[NameFilter] {playerName} was kicked due to a disallowed name."
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
                // Save old name before it changes
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

                if (NameChecker.IsBanned(name, out string matchedWord))
                {
                    Logger?.LogInfo($"[NameFilter] Player changed to banned name: {name}");

                    HudManager.Instance.Chat.AddChat(
                        PlayerControl.LocalPlayer,
                        $"[NameFilter] Warning: {oldName} changed their name to {name} which is disallowed."
                    );
                }
            }
        }
    }
}