using UnityEngine;

namespace _Scripts
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool _firstUpdate = true;

        private void Update()
        {
            if (!_firstUpdate) return;
            _firstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}