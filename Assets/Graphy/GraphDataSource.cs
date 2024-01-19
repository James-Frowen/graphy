using UnityEngine;

namespace JamesFrowen.Graphy
{
    public abstract class GraphDataSource : MonoBehaviour
    {
        /// <summary>
        /// New value from this frame
        /// </summary>
        /// <returns></returns>
        public abstract float GetNewValue();
    }
}
