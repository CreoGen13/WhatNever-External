using Core.Base.Interfaces;
using Core.Game;
using Core.Infrastructure.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using Zenject;

namespace ZenjectInstallers
{
    public class GameContextInstaller : MonoInstaller
    {
        [Header("Game")]
        [SerializeField] private GameView gameView;
        
        [Header("Camera")]
        [SerializeField] private Camera mainCamera;
        
        [Header("Services")]
        [SerializeField] private PlayerInput playerInput;
   
        public override void InstallBindings()
        {
            BindCamera();
            BindServices();
            BindLocalization();

            BindGame();
        }

        private void BindGame()
        {
            Container
                .Bind<GamePresenter>()
                .AsSingle()
                .WithArguments(new GameModel(), gameView)
                .NonLazy();
        }
        private void BindCamera()
        {
            Container
                .Bind<Camera>()
                .FromInstance(mainCamera)
                .AsSingle();
        }
        private void BindServices()
        {
            Container
                .Bind<SoundService>()
                .AsSingle();
                
            Container
                .Bind<LoadService>()
                .AsSingle();
            
            Container
                .Bind<InputService>()
                .AsSingle()
                .WithArguments(playerInput);
            
            Container
                .Bind<SaveService>()
                .AsSingle();
        }
        private void BindLocalization()
        {
            Container
                .Bind<LocalizationSettings>()
                .FromInstance(LocalizationSettings.Instance);
        }
    }
}