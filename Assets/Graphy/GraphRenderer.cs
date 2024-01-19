using System;
using UnityEngine;
using UnityEngine.UI;

namespace JamesFrowen.Graphy
{
    public sealed class GraphRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;
        [SerializeField] private GraphDataSource _dataSource;
        [SerializeField] private GraphText _graphText;
        [SerializeField] private GraphMinMaxText _minMaxText;

        [Header("Graph config")]
        [SerializeField] private Shader _shader;
        [SerializeField] private GraphShader5Levels.Settings _settings;

        private bool _initialized;

        /// <summary>raw values</summary>
        private float[] _array;
        private float _average;
        private float _min;
        private float _max;

        /// <summary>Average of max so it slowly comes back down after going up. Use large window for max, we want it to update slowly</summary>
        private ExponentialMovingAverage _maxAverage = new ExponentialMovingAverage(50);
        /// <summary>Average of min so it slowly comes back down after going up. Use large window for max, we want it to update slowly</summary>
        private ExponentialMovingAverage _minAverage = new ExponentialMovingAverage(50);

        private GraphShader5Levels _shaderGraph = null;

        private void Init()
        {
            _image.material = new Material(_shader);
            _shaderGraph = new GraphShader5Levels(_image, _settings);

            _array = new float[_settings.ArraySize];

            for (var i = 0; i < _array.Length; i++)
            {
                _array[i] = 0f;
            }


            _shaderGraph.UpdateValues(_array, 0, 0, 1);

            _graphText?.Init();

            _initialized = true;
        }

        private void LateUpdate()
        {
            if (!_initialized)
                Init();

            var newValue = _dataSource.GetNewValue();
            UpgradeGraph(newValue);

            _graphText?.UpdateValues(newValue, _average);
            _minMaxText.UpdateValues(_average, _min, _max);
        }

        private void UpgradeGraph(float newValue)
        {
            var currentMax = 0f;
            var currentMin = 0f; // start at zero, but allow it to go negative
            var sum = 0f;

            var length = _array.Length;
            for (var i = 0; i < length; i++)
            {
                // shift values over
                if (i < length - 1)
                {
                    _array[i] = _array[i + 1];
                }
                else // last value
                {
                    _array[i] = newValue;
                }

                // Store the highest fps to use as the highest point in the graph
                currentMax = Math.Max(currentMax, _array[i]);
                currentMin = Math.Min(currentMin, _array[i]);
                // add to sum for average
                sum += _array[i];
            }
            _average = sum / length;

            _minAverage.Add(currentMin);
            _min = Mathf.Min(currentMin, (float)_minAverage.Value);

            _maxAverage.Add(currentMax);
            _max = Mathf.Max(currentMax, (float)_maxAverage.Value);

            _shaderGraph.UpdateValues(_array, _average, _min, _max);
        }
    }
}
