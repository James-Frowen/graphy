using System;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public abstract class GraphText : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected int _percentilesCount = 1000;
        [Tooltip("Get bottom 1% or top 1%")]
        [SerializeField] protected bool _minPercentile = true;
        [SerializeField] protected float _updateInterval = 0.2f;

        /// <summary>raw values used to caculate 1% and 0.1%</summary>
        private float[] _arrayPercentiles;
        private int _percentilesIndex;
        private float _updateTimer;
        // _arrayPercentiles wont start with enough values, so until then use this total
        private int _totalInserted;

        // store sum of values since last text update. then show average of them
        private float _sum;
        private int _count;


        public void Init()
        {
            _arrayPercentiles = new float[_percentilesCount];
            for (var i = 0; i < _percentilesCount; i++)
            {
                _arrayPercentiles[i] = _minPercentile ? float.MaxValue : float.MinValue;
            }
        }
        protected abstract void UpdateText(float newValue, float average);

        public void UpdateValues(float newValue, float average)
        {
            _arrayPercentiles[_percentilesIndex] = newValue;
            _percentilesIndex = (_percentilesIndex + 1) % _percentilesCount;
            _totalInserted = Math.Max(_totalInserted, _percentilesIndex);

            _sum += newValue;
            _count++;

            _updateTimer += Time.unscaledDeltaTime;
            if (_updateTimer > _updateInterval)
            {
                _updateTimer = 0f;

                UpdateText(_sum / _count, average);
                _sum = 0;
                _count = 0;
            }
        }

        protected unsafe float GetPercentile(int percentile)
        {
            var minCounts = _totalInserted / percentile;
            if (minCounts == 0)
                minCounts = 1;

            var mins = stackalloc float[minCounts];
            for (var i = 0; i < minCounts; i++)
            {
                mins[i] = _minPercentile ? float.MaxValue : float.MinValue;
            }

            // loop over values and 
            for (var i = 0; i < _totalInserted; i++)
            {
                var value = _arrayPercentiles[i];
                for (var j = 0; j < minCounts; j++)
                {
                    if (_minPercentile
                        ? value < mins[j]
                        : value > mins[j])
                    {
                        // shift old values
                        // in reverse, or we will set all values to first value
                        for (var k = minCounts - 1; k > j; k--)
                        {
                            mins[k] = mins[k - 1];
                        }

                        // then set new valu
                        mins[j] = value;

                        break;
                    }
                }
            }

            float average = 0;
            for (var i = 0; i < minCounts; i++)
            {
                average += mins[i];
            }
            return average / minCounts;
        }
    }
}
