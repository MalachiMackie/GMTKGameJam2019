using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Activatable : MonoBehaviour
    {
        protected abstract bool _needsActivating { get; set; }

        protected abstract bool Active { get; set; }

        public abstract void Activate();

        public abstract void Deactivate();
    }
}
