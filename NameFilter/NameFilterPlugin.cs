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
        public const string PluginGuid    = "com.namefilter.plugin";
        public const string PluginName    = "NameFilter";
        public const string PluginVersion = "1.0.0";

        internal static Harmony Harmony = new Harmony(PluginGuid);

        public override void Load()
        {
            Harmony.PatchAll();
            Log.LogInfo($"{PluginName} {PluginVersion} laddad!");
        }
    }

    /// <summary>
    /// Hookar in i PlayerJoinedGame och kontrollerar namnet direkt när någon joinar.
    /// </summary>
    [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.HandleMessage))]
    public static class PlayerJoinPatch
    {
        public static void Postfix(LobbyBehaviour __instance)
        {
            // Bara hosten ska köra filtret
            if (!AmongUsClient.Instance.AmHost) return;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                // Hoppa över lokala spelaren (hosten själv)
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
            // Logga i konsolen
            BepInEx.Logging.Logger.Sources[0]?.LogInfo(
                $"[NameFilter] Kickar spelare med otillåtet namn: {playerName}"
            );

            // Skicka kick via Among Us inbyggda system
            AmongUsClient.Instance.KickPlayer(player.GetClientId(), false);

            // Visa meddelande i chatten för alla i lobbyn
            HudManager.Instance.Chat.AddChat(
                PlayerControl.LocalPlayer,
                $"[NameFilter] {playerName} kickades på grund av otillåtet namn."
            );
        }
    }
}
