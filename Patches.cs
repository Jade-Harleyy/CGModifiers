using HarmonyLib;
using UnityEngine;

namespace CGModifiers
{
    [HarmonyPatch]
    internal static class Patches
    {
        [HarmonyPatch(typeof(WaveSetter), "OnPointerClick"), HarmonyPostfix]
        private static void WaveSetter_OnPointerClick(WaveSetter __instance)
        {
            ValueManager.Instance.waveSelect.Value = __instance.wave;
        }

        [HarmonyPatch(typeof(WaveMenu), "SetCurrentWave"), HarmonyPrefix]
        private static bool WaveMenu_SetCurrentWave(int wave, WaveMenu __instance, ref int ___currentWave)
        {
            EndlessGrid.Instance.startWave = ___currentWave = wave;
            PrefsManager.Instance.SetInt("cyberGrind.startingWave", wave);
            foreach (WaveSetter setter in __instance.setters)
            {
                if (setter.wave == ___currentWave)
                {
                    setter.Selected();
                }
                else
                {
                    setter.Unselected();
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(EndlessGrid), "OnTriggerEnter"), HarmonyPrefix]
        private static void EndlessGrid_OnTriggerEnter(Collider other, EndlessGrid __instance, ref int ___maxPoints)
        {
            if (!other.CompareTag("Player")) return;

            ___maxPoints = Mathf.RoundToInt(ValueManager.Instance.startingPointsSelector.Value * (ValueManager.Instance.forceRadianceToggle.Value ? 3 : 1));
            for (int i = 1; i <= __instance.startWave - 1; i++)
            {
                ___maxPoints += Mathf.RoundToInt((3 + i / 3) * (ValueManager.Instance.pointMultiplierSelector.Value * (ValueManager.Instance.forceRadianceToggle.Value ? 3 : 1) - 1));
            }
        }

        [HarmonyPatch(typeof(EndlessGrid), "NextWave"), HarmonyPrefix]
        private static void EndlessGrid_NextWave(EndlessGrid __instance, ref int ___maxPoints)
        {
            ___maxPoints += Mathf.RoundToInt((3 + (__instance.currentWave + 1) / 3) * (ValueManager.Instance.pointMultiplierSelector.Value * (ValueManager.Instance.forceRadianceToggle.Value ? 3 : 1) - 1));
        }

        [HarmonyPatch(typeof(EndlessGrid), "SpawnRadiant"), HarmonyPrefix]
        private static bool EndlessGrid_SpawnRadiant(ref bool __result)
        {
            if (ValueManager.Instance.forceRadianceToggle.Value)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}