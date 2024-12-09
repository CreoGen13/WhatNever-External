using UnityEngine;

namespace Core.Mono
{
    public class ItemFocus : MonoBehaviour
    {
        private Transform _focusItemTransform;

        public void Focus(Transform focusItemTransform)
        {
            _focusItemTransform = focusItemTransform;
        }
        
        public void Update()
        {
            transform.position = _focusItemTransform.position;
        }
    }
}