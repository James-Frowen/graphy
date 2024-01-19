using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace JamesFrowen.Graphy
{
    public class GraphShader5Levels
    {
        private static readonly int AveragePropertyId = Shader.PropertyToID("Average");

        private static readonly int ExcellentThresholdPropertyId = Shader.PropertyToID("_ExcellentThreshold");
        private static readonly int GoodThresholdPropertyId = Shader.PropertyToID("_GoodThreshold");
        private static readonly int NormalThresholdPropertyId = Shader.PropertyToID("_NormalThreshold");
        private static readonly int CautionThresholdPropertyId = Shader.PropertyToID("_CautionThreshold");

        private static readonly int ExcellentColorPropertyId = Shader.PropertyToID("_ExcellentColor");
        private static readonly int GoodColorPropertyId = Shader.PropertyToID("_GoodColor");
        private static readonly int NormalColorPropertyId = Shader.PropertyToID("_NormalColor");
        private static readonly int CautionColorPropertyId = Shader.PropertyToID("_CautionColor");
        private static readonly int CriticalColorPropertyId = Shader.PropertyToID("_CriticalColor");

        private static readonly int GraphValues = Shader.PropertyToID("GraphValues");
        private static readonly int GraphValuesLength = Shader.PropertyToID("GraphValues_Length");

        public const int ArrayMaxSizeFull = 512;

        private readonly bool _lowerIsBetter;
        private readonly float _cautionThreshold;
        private readonly float _goodThreshold;
        private readonly float _normalThreshold;
        private readonly float _excellentThreshold;
        private readonly float[] Array;
        private readonly Material _material;
        private float _range;
        private float _offset;

        [System.Serializable]
        public class Settings
        {
            public int ArraySize = 128;

            [Tooltip("flips what counts as good color")]
            public bool LowerIsBetter = false;

            [Header("Colors")]

            public Color ExcellentColor = Color.cyan;
            public float ExcellentThreshold = 120;

            public Color GoodColor = Color.green;
            public float GoodThreshold = 55;

            public Color NormalColor = Color.yellow;
            public float NormalThreshold = 40;

            public Color CautionColor = new Color(1, 0.5f, 0.1f);
            public float CautionThreshold = 20;

            public Color CriticalColor = Color.red;
        }

        public GraphShader5Levels(
            Image image,
            Settings settings
         )
        {
            if (settings.ArraySize > ArrayMaxSizeFull)
                throw new ArgumentOutOfRangeException(nameof(settings.ArraySize));

            _lowerIsBetter = settings.LowerIsBetter;
            _cautionThreshold = settings.CautionThreshold;
            _goodThreshold = settings.GoodThreshold;
            _normalThreshold = settings.NormalThreshold;
            _excellentThreshold = settings.ExcellentThreshold;

            _material = image.material;
            if (settings.LowerIsBetter)
            {
                _material.SetColor(ExcellentColorPropertyId, settings.CriticalColor);
                _material.SetColor(GoodColorPropertyId, settings.CautionColor);
                _material.SetColor(NormalColorPropertyId, settings.NormalColor);
                _material.SetColor(CautionColorPropertyId, settings.GoodColor);
                _material.SetColor(CriticalColorPropertyId, settings.ExcellentColor);
            }
            else
            {
                _material.SetColor(ExcellentColorPropertyId, settings.ExcellentColor);
                _material.SetColor(GoodColorPropertyId, settings.GoodColor);
                _material.SetColor(NormalColorPropertyId, settings.NormalColor);
                _material.SetColor(CautionColorPropertyId, settings.CautionColor);
                _material.SetColor(CriticalColorPropertyId, settings.CriticalColor);
            }

            Array = new float[settings.ArraySize];

            _material.SetInt(GraphValuesLength, settings.ArraySize);
            _material.SetFloatArray(GraphValues, Array);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float ToGraphValue(float value) => (value - _offset) / _range;

        public void UpdateValues(float[] values, float average, float min, float max)
        {
            // set these values first so ToGraphValue can be used
            _range = max - min;
            _offset = min;

            _material.SetFloat(AveragePropertyId, ToGraphValue(average));
            if (_lowerIsBetter)
            {
                _material.SetFloat(ExcellentThresholdPropertyId, ToGraphValue(_cautionThreshold));
                _material.SetFloat(GoodThresholdPropertyId, ToGraphValue(_normalThreshold));
                _material.SetFloat(NormalThresholdPropertyId, ToGraphValue(_goodThreshold));
                _material.SetFloat(CautionThresholdPropertyId, ToGraphValue(_excellentThreshold));
            }
            else
            {
                _material.SetFloat(ExcellentThresholdPropertyId, ToGraphValue(_excellentThreshold));
                _material.SetFloat(GoodThresholdPropertyId, ToGraphValue(_goodThreshold));
                _material.SetFloat(NormalThresholdPropertyId, ToGraphValue(_normalThreshold));
                _material.SetFloat(CautionThresholdPropertyId, ToGraphValue(_cautionThreshold));
            }

            // copy to graph, as percent of max
            for (var i = 0; i < values.Length; i++)
            {
                Array[i] = ToGraphValue(values[i]);
            }
            _material.SetFloatArray(GraphValues, Array);
        }
    }
}
