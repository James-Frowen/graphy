using JamesFrowen.ScriptableVariables;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class FrameTimeGraphText : GraphText
    {
        [Header("References")]
        [SerializeField] private NonAllocGui.Wrapper _ms;
        [SerializeField] private NonAllocGui.Wrapper _avg;
        [SerializeField] private NonAllocGui.Wrapper _onePercent;
        [SerializeField] private NonAllocGui.Wrapper _zero1Percent;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                _ms.Init();
                _avg.Init();
                _onePercent.Init();
                _zero1Percent.Init();
            }
#endif
        }

        protected override void UpdateText(float newValue, float average)
        {
            _ms.SetValue(newValue);
            _avg.SetValue(average);

            var onePercent = GetPercentile(100);
            _onePercent.SetValue(onePercent);

            var zeroOnePercewnt = GetPercentile(1000);
            _zero1Percent.SetValue(zeroOnePercewnt);
        }
    }
}
