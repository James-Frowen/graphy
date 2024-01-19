using UnityEngine;

namespace JamesFrowen.Graphy
{
    public class FpsGraphData : GraphDataSource
    {
        public override float GetNewValue()
        {
            var deltaTime = Time.unscaledDeltaTime;

            return 1f / deltaTime;
        }
    }
}
