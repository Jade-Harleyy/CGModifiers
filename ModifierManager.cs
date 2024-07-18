using HarmonyLib;
using UnityEngine;

namespace CGModifiers
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class ModifierManager : MonoSingleton<ModifierManager>
    {
        private WaveMenu wm;

        // Wave Select
        public FloatSelector waveSelect;

        // Wave Modifiers
        public FloatSelector startingPointsSelector, pointMultiplierSelector;

        // Enemy Modifiers
        //public StringSelector forceRadianceSelector;
        public BoolSelector forceRadianceSelector;

        // Arena Modifiers
        public BoolSelector scoreboardToggler;
        public FloatSelector zapZoneHeightSelector;

        private void Start()
        {
            wm = waveSelect.GetComponentInParent<WaveMenu>();

            waveSelect.maxValue = Mathf.Min(Traverse.Create(wm).Field("highestWave").GetValue<int>() / 2, 25);
            startingPointsSelector.minValue = Traverse.Create(EndlessGrid.Instance).Field("maxPoints").GetValue<int>();

            waveSelect.onValueChanged += (value) => wm.SetCurrentWave((int)value);
        }
    }
}