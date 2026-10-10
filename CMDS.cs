using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace CMDS;

[BepInPlugin("Lukiiy.CMDS", "CMDS", "1.0")]
public class CMDS : BaseUnityPlugin
{
    internal static CMDS Instance { get; private set; } = null!;
    internal static ManualLogSource Log { get; private set; } = null!;

    private void Awake()
    {
        Instance = this;
        Log = Logger;

        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        new Harmony(Info.Metadata.GUID).PatchAll();
    }

    [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.ChatMessageSend))]
    internal static class SendPatch
    {
        private static bool Prefix(PlayerAvatar __instance, string _message)
        {
            if (__instance != PlayerAvatar.instance || !SemiFunc.IsMasterClientOrSingleplayer()) return true; // only client msgs & only host

            return !CommandAPI.TryHandle(_message);
        }
    }
}