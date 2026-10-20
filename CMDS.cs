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

        CommandAPI.Register(new Kill());
        CommandAPI.Register(new Revive());
        CommandAPI.Register(new Dump());

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

    [HarmonyPatch(typeof(ChatManager), "Update")]
    internal static class SingleplayerPatch
    {
        private static readonly MethodInfo isOnline = AccessTools.Method(typeof(SemiFunc), nameof(SemiFunc.IsMultiplayer));

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> code)
        {
            foreach (var ci in code)
            {
                if (ci.Calls(isOnline))
                {
                    ci.opcode = OpCodes.Ldc_I4_1;
                    ci.operand = null;
                }

                yield return ci;
            }
        }
    }
}