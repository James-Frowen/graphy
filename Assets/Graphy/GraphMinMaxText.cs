using JamesFrowen.ScriptableVariables;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public sealed class GraphMinMaxText : MonoBehaviour
    {
        [SerializeField] private RectTransform _avgHolder;
        [SerializeField] private float _bottom;
        [SerializeField] private float _top;

        [Header("References")]
        [SerializeField] private NonAllocGui.Wrapper _avgText;
        [SerializeField] private NonAllocGui.Wrapper _minText;
        [SerializeField] private NonAllocGui.Wrapper _maxText;


        public void UpdateValues(float avg, float min, float max)
        {
            _avgText.SetValue(avg);
            _minText.SetValue(min);
            _maxText.SetValue(max);


            var t = Mathf.InverseLerp(min, max, avg);
            var y = Mathf.Lerp(_bottom, _top, t);
            _avgHolder.anchoredPosition = new Vector2(_avgHolder.anchoredPosition.x, y);
        }
    }
}
