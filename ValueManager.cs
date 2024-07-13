using HarmonyLib;
using UnityEngine;

namespace CGModifiers
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class ValueManager : MonoSingleton<ValueManager>
    {
        private WaveMenu wm;

        public ValueSelector waveSelect, startingPointsSelector, pointMultiplierSelector;
        public ValueToggler forceRadianceToggle;

        private void Start()
        {
            wm = waveSelect.GetComponentInParent<WaveMenu>();

            waveSelect.maxValue = Mathf.Min(Traverse.Create(wm).Field("highestWave").GetValue<int>() / 2, 25);
            startingPointsSelector.minValue = Traverse.Create(EndlessGrid.Instance).Field("maxPoints").GetValue<int>();

            waveSelect.onValueChanged += (value) => wm.SetCurrentWave((int)value);
        }
    }
}
