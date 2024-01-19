using System.ComponentModel;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class FrameTimeGraphData : GraphDataSource
    {
        [SerializeField] private Source _source;
        private readonly FrameTiming[] _frames = new FrameTiming[1];

        public override float GetNewValue()
        {
            if (_source == Source.Time)
                return Time.unscaledDeltaTime * 1000;


            var count = FrameTimingManager.GetLatestTimings(1, _frames);
            if (count == 0)
                return 0;

            var f = _frames[0];
            return (float)GetValue(f);
        }

        private double GetValue(FrameTiming f)
        {
            switch (_source)
            {
                case Source.cpuFrameTime: return f.cpuFrameTime;
                case Source.cpuMainThreadFrameTime: return f.cpuMainThreadFrameTime;
                case Source.cpuMainThreadPresentWaitTime: return f.cpuMainThreadPresentWaitTime;
                case Source.cpuRenderThreadFrameTime: return f.cpuRenderThreadFrameTime;
                case Source.gpuFrameTime: return f.gpuFrameTime;
                default: throw new InvalidEnumArgumentException();
            }
        }

        [System.Serializable]
        public enum Source
        {
            Time,
            cpuFrameTime,
            cpuMainThreadFrameTime,
            cpuMainThreadPresentWaitTime,
            cpuRenderThreadFrameTime,
            gpuFrameTime,
        }
    }
}
