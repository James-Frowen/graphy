using Unity.Profiling;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class ProfilerMarkerGraphData : GraphDataSource
    {
        [SerializeField] private string _markerName = "PlayerLoop";
        [SerializeField] private ProfilerCategory _category = ProfilerCategory.Scripts;

        private ProfilerRecorder _recorder;

        private void OnEnable()
        {
            StartRecorder();
        }

        private void OnDisable()
        {
            StopRecorder();
        }

        public override float GetNewValue()
        {
            if (!_recorder.Valid)
                return 0f;

            // ProfilerRecorder reports timing in nanoseconds, convert to milliseconds
            return _recorder.LastValue / 1_000_000f;
        }

        /// <summary>
        /// Changes the profiler marker and category being recorded at runtime.
        /// </summary>
        /// <param name="markerName">The name of the profiler marker to record.</param>
        /// <param name="category">The profiler category the marker belongs to.</param>
        public void SetMarker(string markerName, ProfilerCategory category)
        {
            _markerName = markerName;
            _category = category;

            if (isActiveAndEnabled)
            {
                StopRecorder();
                StartRecorder();
            }
        }

        private void StartRecorder()
        {
            if (string.IsNullOrEmpty(_markerName))
                return;

            _recorder = ProfilerRecorder.StartNew(_category, _markerName);
        }

        private void StopRecorder()
        {
            if (_recorder.Valid)
                _recorder.Dispose();
        }
    }
}
