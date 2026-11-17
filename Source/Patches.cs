using System;
using UnityEngine;
using HarmonyLib;
using NineSolsAPI;
using Cysharp.Threading.Tasks;
using System.Security;

namespace FreezeFrames;

[HarmonyPatch]
public static class Patches {

    static bool delay = true;
    static int frameCount = 0;

    [HarmonyPatch(typeof(RCGTime), nameof(RCGTime.UnscaledDelay))]
    [HarmonyPrefix]
    public static void UnscaledDelay(this MonoBehaviour mb, float second) {
        delay = !delay;
        if (delay) {
            int freezeFrames = (int) Math.Round(second * 60);
            frameCount += freezeFrames;
            ToastManager.Toast(freezeFrames + " freeze frames, total " + frameCount + " since last reset");
        }
    }

    /* Monster-specific freeze
    [HarmonyPatch(typeof(MonsterBase), nameof(MonsterBase.FreezeFor))]
    [HarmonyPrefix]
    public static void FreezeFor(float seconds)
    {
        int freezeFrames = (int) Math.Round(seconds * 60);
        frameCount += freezeFrames;
        ToastManager.Toast(freezeFrames + " freeze frames, total " + frameCount + " since last reset");
    }
    */

    [HarmonyPatch(typeof(MonsterBase), nameof(MonsterBase.FreezeFor))]
    [HarmonyPrefix]
    public static void FreezeFor(this MonsterBase __instance, float seconds)
    {
        if (__instance is BossBase bb)
        {
            ToastManager.Toast(bb.BossName);
        }
    }

    public static void resetFrameCount() {
        ToastManager.Toast("Frame count reset at " + frameCount + " frames.");
        frameCount = 0;
    }
}