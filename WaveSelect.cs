using HarmonyLib;
using TMPro;
using UnityEngine;

namespace CGModifiers
{
    public class WaveSelect : MonoBehaviour
    {
        public TMP_Text waveText;

        private WaveMenu wm;
        private int maxWave;

        private int Wave
        {
            get => Traverse.Create(wm).Field("currentWave").GetValue<int>();
            set => wm.SetCurrentWave(Mathf.Clamp(value, 1, maxWave));
        }

        private void Start()
        {
            wm = GetComponentInParent<WaveMenu>();
            waveText.text = PrefsManager.Instance.GetInt("cyberGrind.startingWave").ToString();
        }

        private void Update()
        {
            int oldMaxWave = Traverse.Create(wm).Field("highestWave").GetValue<int>() / 2;
            maxWave = AssistController.Instance.cheatsEnabled ? int.MaxValue : StatsManager.Instance.majorUsed ? oldMaxWave : Mathf.Min(oldMaxWave, 25);
            waveText.text = Wave.ToString();
        }

        public void AddAmount(int amount) => Wave += InputManager.Instance.InputSource.Dodge.IsPressed ? amount * 10 : amount;
    }
}