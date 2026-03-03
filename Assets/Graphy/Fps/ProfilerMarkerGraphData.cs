using Unity.Profiling;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class ProfilerMarkerGraphData : GraphDataSource
    {
        [SerializeField] private string _markerName = "PlayerLoop";
        [SerializeField] private ProfilerCategoryEnum _category = ProfilerCategoryEnum.Scripts;

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
        public void SetMarker(string markerName, ProfilerCategoryEnum category)
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

            ProfilerCategory category = _category switch
            {
                ProfilerCategoryEnum.Render => ProfilerCategory.Render,
                ProfilerCategoryEnum.Scripts => ProfilerCategory.Scripts,
                ProfilerCategoryEnum.Gui => ProfilerCategory.Gui,
                ProfilerCategoryEnum.Physics => ProfilerCategory.Physics,
                ProfilerCategoryEnum.Physics2D => ProfilerCategory.Physics2D,
                ProfilerCategoryEnum.Animation => ProfilerCategory.Animation,
                ProfilerCategoryEnum.Ai => ProfilerCategory.Ai,
                ProfilerCategoryEnum.Audio => ProfilerCategory.Audio,
                ProfilerCategoryEnum.Video => ProfilerCategory.Video,
                ProfilerCategoryEnum.Particles => ProfilerCategory.Particles,
                ProfilerCategoryEnum.Lighting => ProfilerCategory.Lighting,
                ProfilerCategoryEnum.Network => ProfilerCategory.Network,
                ProfilerCategoryEnum.Loading => ProfilerCategory.Loading,
                ProfilerCategoryEnum.Vr => ProfilerCategory.Vr,
                ProfilerCategoryEnum.Input => ProfilerCategory.Input,
                ProfilerCategoryEnum.Memory => ProfilerCategory.Memory,
                ProfilerCategoryEnum.VirtualTexturing => ProfilerCategory.VirtualTexturing,
                ProfilerCategoryEnum.FileIO => ProfilerCategory.FileIO,
                ProfilerCategoryEnum.Internal => ProfilerCategory.Internal,
                _ => ProfilerCategory.Scripts
            };

            _recorder = ProfilerRecorder.StartNew(category, _markerName);
        }

        private void StopRecorder()
        {
            if (_recorder.Valid)
                _recorder.Dispose();
        }
    }

    [System.Serializable]
    public enum ProfilerCategoryEnum : ushort
    {
        Render = 0,
        Scripts = 1,
        Gui = 4,
        Physics = 5,
        Physics2D = 33,
        Animation = 6,
        Ai = 7,
        Audio = 8,
        Video = 11,
        Particles = 12,
        Lighting = 13,
        Network = 14,
        Loading = 15,
        Vr = 22,
        Input = 30,
        Memory = 23,
        VirtualTexturing = 31,
        FileIO = 25,
        Internal = 24,
    }
}
