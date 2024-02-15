using JamesFrowen.ScriptableVariables;
using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class FrameTimingText : MonoBehaviour
    {
        [SerializeField] protected float _updateInterval = 0.2f;

        [Header("References")]
        [SerializeField] private NonAllocGui.Wrapper _cpuFrameTimeText;
        [SerializeField] private NonAllocGui.Wrapper _cpuMainThreadFrameTimeText;
        [SerializeField] private NonAllocGui.Wrapper _cpuMainThreadPresentWaitTimeText;
        [SerializeField] private NonAllocGui.Wrapper _cpuRenderThreadFrameTimeText;
        [SerializeField] private NonAllocGui.Wrapper _gpuFrameTimeText;

        private readonly FrameTiming[] _frames = new FrameTiming[1];
        private float _updateTimer;
        private int _count;
        private float _cpuFrameTimeSum;
        private float _cpuMainThreadFrameTimeSum;
        private float _cpuMainThreadPresentWaitTimeSum;
        private float _cpuRenderThreadFrameTimeSum;
        private float _gpuFrameTimeSum;

        public void Update()
        {
            _updateTimer += Time.unscaledDeltaTime;

            var count = FrameTimingManager.GetLatestTimings(1, _frames);
            if (count == 0)
                return;

            var f = _frames[0];
            _cpuFrameTimeSum += (float)f.cpuFrameTime;
            _cpuMainThreadFrameTimeSum += (float)f.cpuMainThreadFrameTime;
            _cpuMainThreadPresentWaitTimeSum += (float)f.cpuMainThreadPresentWaitTime;
            _cpuRenderThreadFrameTimeSum += (float)f.cpuRenderThreadFrameTime;
            _gpuFrameTimeSum += (float)f.gpuFrameTime;
            _count++;

            if (_updateTimer > _updateInterval)
            {
                _updateTimer = 0f;

                UpdateText();
                _cpuFrameTimeSum = 0;
                _cpuMainThreadFrameTimeSum = 0;
                _cpuMainThreadPresentWaitTimeSum = 0;
                _cpuRenderThreadFrameTimeSum = 0;
                _gpuFrameTimeSum = 0;
                _count = 0;
            }
        }

        private void UpdateText()
        {
            _cpuFrameTimeText.SetValue(_cpuFrameTimeSum / _count);
            _cpuMainThreadFrameTimeText.SetValue(_cpuMainThreadFrameTimeSum / _count);
            _cpuMainThreadPresentWaitTimeText.SetValue(_cpuMainThreadPresentWaitTimeSum / _count);
            _cpuRenderThreadFrameTimeText.SetValue(_cpuRenderThreadFrameTimeSum / _count);
            _gpuFrameTimeText.SetValue(_gpuFrameTimeSum / _count);
        }
    }
}
