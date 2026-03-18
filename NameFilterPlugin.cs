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

        public override void Load()
        {
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
        }

        /// <summary>
        /// Hooks into LobbyBehaviour.Start and checks player names when the lobby starts.
        /// </summary>
        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        public static class PlayerJoinPatch
        {
            public static void Postfix(LobbyBehaviour __instance)
            {
                // Only the host should run the filter
                if (!AmongUsClient.Instance.AmHost) return;

                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    // Skip the host themselves
                    if (player.IsLocal) continue;

                    string playerName = player.Data.PlayerName;

                    if (NameChecker.IsBanned(playerName, out string matchedWord))
                    {
                        KickPlayer(player, playerName);
                    }
                }
            }

            private static void KickPlayer(PlayerControl player, string playerName)
            {
                // Log to console
                BepInEx.Logging.Logger.Sources[0]?.LogInfo(
                    $"[NameFilter] Kicking player with disallowed name: {playerName}"
                );

                // Kick via Among Us built-in system
                AmongUsClient.Instance.KickPlayer(player.GetClientId(), false);

                // Show message in chat - only visible to host
                HudManager.Instance.Chat.AddChat(
                    PlayerControl.LocalPlayer,
                    $"[NameFilter] {playerName} was kicked due to a disallowed name."
                );
            }
        }
    }
}