using Scriptables.Holders;
using Scriptables.Settings;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [Header("Settings")]
        [SerializeField] private ScriptableGameSettings gameSettings;
        [SerializeField] private ScriptableProjectSettings projectSettings;
        [SerializeField] private ScriptableAudioSettings audioSettings;
        [SerializeField] private ScriptableUiSettings uiSettings;
        [SerializeField] private ScriptableInputSettings inputSettings;
        
        [Header("Holders")]
        [SerializeField] private ScriptableLocalizationHolder localizationHolder;
        [SerializeField] private ScriptableBubblesSpriteHolder bubblesSpriteHolder;
        [SerializeField] private ScriptableSoundsHolder soundsHolder;
        
        public override void InstallBindings()
        {
            BindScriptables();
            BindHolders();

            SetupApplication();
        }

        private void BindScriptables()
        {
            Container
                .Bind<ScriptableGameSettings>()
                .FromInstance(gameSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableProjectSettings>()
                .FromInstance(projectSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableAudioSettings>()
                .FromInstance(audioSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableUiSettings>()
                .FromInstance(uiSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableInputSettings>()
                .FromInstance(inputSettings)
                .AsSingle();
        }
        private void BindHolders()
        {
            Container
                .Bind<ScriptableBubblesSpriteHolder>()
                .FromInstance(bubblesSpriteHolder)
                .AsSingle();
            
            Container
                .Bind<ScriptableSoundsHolder>()
                .FromInstance(soundsHolder)
                .AsSingle();
            
            Container
                .Bind<ScriptableLocalizationHolder>()
                .FromInstance(localizationHolder)
                .AsSingle();
        }

        private void SetupApplication()
        {
            Application.targetFrameRate = projectSettings.targetFrameRate;
        }
    }
}