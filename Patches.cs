using HarmonyLib;
using TMPro;
using UnityEngine;

namespace CGModifiers
{
    [HarmonyPatch]
    internal static class Patches
    {
        [HarmonyPatch(typeof(WaveMenu), "Start"), HarmonyPrefix]
        private static void WaveMenu_Start(WaveMenu __instance)
        {
            Transform wavesPanel = __instance.transform.Find("Canvas/Waves/Panel");
            Object.Instantiate(Plugin.assets.LoadAsset<GameObject>("Wave Select"), wavesPanel, false);

            Transform text = wavesPanel.Find("Text");
            text.localPosition += new Vector3(0, 10, 0);
            text.GetComponent<TextMeshProUGUI>().paragraphSpacing = -30;

            wavesPanel.Find("0").GetComponent<WaveSetter>().wave = 1;
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
    }
}