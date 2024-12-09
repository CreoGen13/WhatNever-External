using UnityEngine;

namespace Core.Mono
{
    [RequireComponent(typeof(RectTransform))]
    public class PointHolder : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
    }
}