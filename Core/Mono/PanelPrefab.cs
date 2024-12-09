using Core.Infrastructure.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Mono
{
    public class PanelPrefab : MonoBehaviour
    {
        [SerializeField] private GameObject endScroll;
        [SerializeField] private GameObject contentPrefab;
        [SerializeField] private Stage stage;
        
        public GameObject ContentPrefab => contentPrefab;
        public Stage Stage => stage;
        
        private FadeHelper[] _fadeHelpers;
        private PanelType _panelType;

        public void Init(PanelType panelType)
        {
            _panelType = panelType;

            if (_panelType == PanelType.Zoom)
            {
                _fadeHelpers = GetComponentsInChildren<FadeHelper>();
            }
        }

        public void InitFadeHelpers(float transparencyStartDistance, float transparencyEndDistance)
        {
            if (_panelType != PanelType.Zoom)
            {
                return;
            }
            
            foreach (var fadeHelper in _fadeHelpers)
            {
                fadeHelper.Init(transparencyStartDistance, transparencyEndDistance);
            }
        }
        
        public void UpdateFadeHelpers(float mainCameraPosition)
        {
            if (_panelType != PanelType.Zoom)
            {
                return;
            }
            
            foreach (var fadeHelper in _fadeHelpers)
            {
                fadeHelper.UpdateAlpha(mainCameraPosition);
            }
        }
        
        public float GetLowerBound()
        {
            return _panelType switch
            {
                PanelType.Vertical => -endScroll.transform.position.y,
                PanelType.Horizontal => -endScroll.transform.position.x,
                PanelType.Zoom => -endScroll.transform.position.z,
                _ => 0
            };
        }
    }
}