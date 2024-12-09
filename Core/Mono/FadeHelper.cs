using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Mono
{
    [RequireComponent(typeof(Image))]
    public class FadeHelper : MonoBehaviour
    {
        [SerializeField] private bool useDefaultSettings = true;
        [HideIf("useDefaultSettings")]
        [SerializeField] private float zoomSpritesTransparencyStartDistance;
        [HideIf("useDefaultSettings")]
        [SerializeField] private float zoomSpritesTransparencyEndDistance;
        private float _distanceDifference;
        
        private Image _image;

        public void Init(float transparencyStartDistance, float transparencyEndDistance)
        {
            if (useDefaultSettings)
            {
                zoomSpritesTransparencyStartDistance = transparencyStartDistance;
                zoomSpritesTransparencyEndDistance = transparencyEndDistance;
            } 
            _distanceDifference = zoomSpritesTransparencyStartDistance - zoomSpritesTransparencyEndDistance;
        }

        public void UpdateAlpha(float cameraZCoordinate)
        {
            var distance = Mathf.Abs(_image.transform.position.z - cameraZCoordinate);

            if (distance > zoomSpritesTransparencyStartDistance)
            {
                var opaque = _image.color;
                opaque.a = 1;
                _image.color = opaque;
                return;
            }

            var newColor = _image.color;
            newColor.a = Mathf.Clamp(
                (distance - zoomSpritesTransparencyEndDistance) / _distanceDifference,
                0,
                1);

            _image.color = newColor;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}