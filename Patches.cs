using HarmonyLib;
using UnityEngine;

namespace CGModifiers
{
    [HarmonyPatch]
    internal static class Patches
    {
        [HarmonyPatch(typeof(WaveSetter), "OnPointerClick"), HarmonyPostfix]
        private static void WaveSetter_OnPointerClick_Postfix(WaveSetter __instance)
        {
            ModifierManager.Instance.waveSelect.Value = __instance.wave;
        }

        [HarmonyPatch(typeof(WaveMenu), "SetCurrentWave"), HarmonyPrefix]
        private static bool WaveMenu_SetCurrentWave_Prefix(int wave, WaveMenu __instance, ref int ___currentWave)
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
        private static void EndlessGrid_OnTriggerEnter_Prefix(Collider other, EndlessGrid __instance, ref int ___maxPoints)
        {
            if (!other.CompareTag("Player")) return;

            ___maxPoints = Mathf.RoundToInt(ModifierManager.Instance.startingPointsSelector.Value * (ModifierManager.Instance.forceRadianceSelector.Value/* == "Always On"*/ ? 3 : 1));
            for (int i = 1; i <= __instance.startWave - 1; i++)
            {
                ___maxPoints += Mathf.RoundToInt((3 + i / 3) * (ModifierManager.Instance.pointMultiplierSelector.Value * (ModifierManager.Instance.forceRadianceSelector.Value/* == "Always On"*/ ? 3 : 1) - 1));
            }

            Transform zapZone = GameObject.Find("/Everything").transform.GetChild(4);
            zapZone.localPosition = new(zapZone.localPosition.x, ModifierManager.Instance.zapZoneHeightSelector.Value * 2.5f + 10.5f, zapZone.localPosition.z);

            GameObject.Find("/Everything").transform.GetChild(4);
        }

        [HarmonyPatch(typeof(EndlessGrid), "OnTriggerEnter"), HarmonyPostfix]
        private static void EndlessGrid_OnTriggerEnter_Postfix(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            GameObject.Find("/Everything").transform.GetChild(4).gameObject.SetActive(ModifierManager.Instance.scoreboardToggler.Value);
        }


        [HarmonyPatch(typeof(EndlessGrid), "NextWave"), HarmonyPrefix]
        private static void EndlessGrid_NextWave_Prefix(EndlessGrid __instance, ref int ___maxPoints)
        {
            ___maxPoints += Mathf.RoundToInt((3 + (__instance.currentWave + 1) / 3) * (ModifierManager.Instance.pointMultiplierSelector.Value * (ModifierManager.Instance.forceRadianceSelector.Value/* == "Always On"*/ ? 3 : 1) - 1));
        }

        [HarmonyPatch(typeof(EndlessGrid), "SpawnRadiant"), HarmonyPrefix]
        private static bool EndlessGrid_SpawnRadiant_Prefix(ref bool __result)
        {
            /*
            switch (ModifierManager.Instance.forceRadianceSelector.Value)
            {
                case "Always Off":
                    __result = false;
                    return false;
                case "Always On":
                    __result = true;
                    return false;
            }
            */
            if (ModifierManager.Instance.forceRadianceSelector.Value)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}