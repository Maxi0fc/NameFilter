using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

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
        internal static BepInEx.Logging.ManualLogSource Logger;

        public override void Load()
        {
            Logger = Log;
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
        }

        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        public static class PlayerJoinPatch
        {
            public static void Postfix(LobbyBehaviour __instance)
            {
                if (!AmongUsClient.Instance.AmHost) return;

                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    // Skip the host themselves
                    if (player.AmOwner) continue;

                    string playerName = player.Data.PlayerName;

                    if (NameChecker.IsBanned(playerName, out string matchedWord))
                    {
                        KickPlayer(player, playerName);
                    }
                }
            }

            private static void KickPlayer(PlayerControl player, string playerName)
            {
                NameFilterPlugin.Logger?.LogInfo(
                    $"[NameFilter] Kicking player with disallowed name: {playerName}"
                );

                AmongUsClient.Instance.KickPlayer(player.GetClientId(), false);

                HudManager.Instance.Chat.AddChat(
                    PlayerControl.LocalPlayer,
                    $"[NameFilter] {playerName} was kicked due to a disallowed name."
                );
            }
        }
    }
}