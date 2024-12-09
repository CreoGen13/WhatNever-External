using System;
using System.Linq;
using Core.Base.Classes;
using Core.Base.Interfaces;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Services;
using Core.Infrastructure.Utils;
using Core.Node.Plot;
using Cysharp.Threading.Tasks;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Core.Game
{
    public class GamePresenter : BasePresenter<GameModel, GameView>
    {
        #region EVENTS
        
        public event Action OnPanelEnd;

        #endregion
        #region VARIABLES

        private readonly MainMenuPresenter _mainMenuPresenter;
        private readonly NodeFactory _nodeFactory;
        private readonly InputService _inputService;
        private readonly LoadService _loadService;
        private readonly SaveService _saveService;
        private readonly SoundService _soundService;
        private readonly GamePresenter _gamePresenter;

        #endregion
        #region PROPERTIES

        public GameView GameView => View;
        public GameModel GameModel => Model;

        #endregion
        
        #region INIT
        
        [Inject]
        public GamePresenter(GameModel model, GameView view,
            MainMenuPresenter mainMenuPresenter,
            NodeFactory nodeFactory,
            InputService inputService,
            LoadService loadService,
            SaveService saveService,
            SoundService soundService)
            : base(model, view)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _nodeFactory = nodeFactory;

            _inputService = inputService;
            _loadService = loadService;
            _saveService = saveService;
            _soundService = soundService;
            
            Model.Services.Add(inputService);
            Model.Services.Add(loadService);
            Model.Services.Add(saveService);
            Model.Services.Add(soundService);
            Model.Update();

            InitSubscriptions();
            InitActions();
            
            this.Log("Game initialized");
        }

        protected sealed override void InitSubscriptions()
        {
            AddSubscription(model => model.IsBlocked, null);
            AddDictionarySubscription(model => model.ItemVariables, (variable, _, newValue) =>
            {
                _mainMenuPresenter.OnItemVariableChanged(variable, newValue);
            });
            AddDictionarySubscription(model => model.GameVariables, (variable, _, newValue) =>
            {
                _mainMenuPresenter.OnGameVariableChanged(variable, newValue);
            });
            AddDictionarySubscription(model => model.PosterPartVariables, (_, _, _) =>
            {
                var hasAllPosterParts = Model.PosterPartVariables.All(posterPart => posterPart.Value);
                
                Model.GameVariables[GameVariable.HasAllPosterParts] = hasAllPosterParts;
                Model.Update();
            });
        }
        protected sealed override void InitActions()
        {
            View.OnAwake += async () =>
            {
                await Start();
            };
            View.OnUpdate += async () =>
            {
                await Update();
            };
            View.OnExit += Clear;
            
            _mainMenuPresenter.StartGameEvent += async onComplete =>
            {
                await StartGame();
                onComplete?.Invoke();
            };
            _mainMenuPresenter.PauseGameEvent += value =>
            {
                Model.IsPaused = value;
                Model.Update();
            };
            _mainMenuPresenter.EndGameEvent += OnEndGame;

            OnPanelEnd += async () =>
            {
                await LoadNextPanel();
            };
        }

        private async UniTask Start()
        {
            foreach (var service in Model.Services)
            {
                if (service is IAsyncService asyncService)
                {
                    await asyncService.Initialize();
                }
                else
                {
                    service.Initialize();
                }
            }
            
            var plot = await _loadService.LoadPlot();
            Model.IsBlocked = true;
            Model.SetPlot(plot);
            Model.Update();
            
            this.Log("Game started");
            
            _mainMenuPresenter.Start();
        }
        
        #endregion
        #region MAIN
        
        private async UniTask StartGame()
        {
            await LoadSpecificPanel("START", true);
        }
        
        private async UniTask Update()
        {
            foreach (var service in Model.Services)
            {
                service.Update();
            }
            
            if (!Model.IsBusy)
            {
                var input = _inputService.GetInput();
                
                switch (input.InputType)
                {
                    case InputService.InputType.None:
                    {
                        await CheckInstantNextMove();
                        
                        break;
                    }
                    case InputService.InputType.Click:
                    {
                        await CheckClicking();
                        
                        break;
                    }
                    case InputService.InputType.Scroll:
                    {
                        await CheckScrolling(input.Delta);
                        
                        break;
                    }
                }
            }
            
            View.ManualUpdate();
        }

        private void OnEndGame()
        {
            foreach (var gameSprite in Model.GameSpritesDictionary.Values)
            {
                gameSprite.DisableAndReturn(false);
            }
            
            View.Clear();
            Model.Clear();
            
            _loadService.UnloadAllPanelData();
            _mainMenuPresenter.Clear();
            
            this.Log("Game ended");
        }

        private void Clear()
        {
            _loadService.UnloadAll();
        }

        #endregion
        
        #region LOAD LOGIC
        
        private async UniTask LoadSpecificPanel(string name, bool next = false)
        {
            var loaded = Model.LoadSpecificPanel(name, out var panelData);

            if (next)
            {
                Model.LoadNextPanel(out panelData);
            }
            
            if (loaded)
            {
                await LoadPanel(panelData);
            }
        }
        private async UniTask LoadNextPanel()
        {
            var loaded = Model.LoadNextPanel(out var panelData);
            
            if (loaded)
            {
                await LoadPanel(panelData);
            }
            else
            {
                this.Log("No panels left");
                Model.SetPanel(null);
            }

            Model.InstantNextMove = true;
            Model.Update();
        }
        private async UniTask LoadPanel(PanelData panelData)
        {
            this.Log("Started loading panel " + panelData.name + " of type " + panelData.panelType);
            
            _mainMenuPresenter.ClearSentences();
            
            Model.SetPanel(panelData);
            Model.Update();
            
            var loadedPanel = await _loadService.LoadPanel(panelData);
            SetLoadedPanel(loadedPanel);
        }

        // private void UnloadCurrentPanel(bool deleteVisual)
        // {
        //     if (Model.CurrentPanelData == null)
        //     {
        //         return;
        //     }
        //
        //     var nodes = Model.GetAllNodeData();
        //
        //     foreach (var nodeData in nodes)
        //     {
        //         var sprite = Model.GameSprites.FirstOrDefault(pair => pair.Key.name == nodeData.name);
        //
        //         if (sprite.Value != null)
        //         {
        //             if (deleteVisual)
        //             {
        //                 Model.GameSprites[sprite.Key].DisableAndReturn(false);
        //             }
        //         
        //             Model.GameSprites.Remove(sprite.Key);
        //         }
        //         
        //         
        //         _loadService.UnloadNodeData(nodeData);
        //     }
        //     
        //     _loadService.UnloadPanel(Model.CurrentPanelData);
        // }
        
        private void SetLoadedPanel(LoadedPanelData loadedPanel)
        {
            if (loadedPanel.HasAudio)
            {
                _soundService.SetMusic(loadedPanel.Audio);
            }

            if (loadedPanel.HasPrefab)
            {
                View.SpawnPanelPrefab(loadedPanel.Prefab, loadedPanel.PanelType);
            }
            
            Model.IsBlocked = false;
            Model.SetLoadedPanel(loadedPanel);
            Model.Update();

            if (loadedPanel.DeletePreviousPanel)
            {
                View.DestroyPanels();
            }
            
            View.PlayZoom(Vector3.zero, 1, null, true);
            
            this.Log("Finished loading panel " + loadedPanel.Name + " of type " + loadedPanel.PanelType);
        }

        #endregion
        #region PANEL LOGIC
        
        private async UniTask CheckInstantNextMove()
        {
            if (Model.InstantNextMove)
            {
                switch (Model.CurrentPanelType)
                {
                    case PanelType.Clicking:
                    {
                        await CheckClicking();
                        
                        break;
                    }
                    case PanelType.Horizontal:
                    case PanelType.Vertical:
                    case PanelType.Zoom:
                    {
                        await CheckScrolling(Vector2.zero);
                        
                        break;
                    }
                }

                Model.InstantNextMove = false;
                Model.Update();
            }
        }
        private async UniTask CheckScrolling(Vector2 input)
        {
            if (Model.CurrentPanelType != PanelType.Vertical && Model.CurrentPanelType != PanelType.Horizontal && Model.CurrentPanelType != PanelType.Zoom)
            {
                return;
            }
            
            if (!Model.FirstScroll)
            {
                Model.FirstScroll = true;
                Model.Update();
                    
                Model.StartDeferredEvent(DeferredEventType.ScrollStarted, null);
            }
            
            var isScrollingEnded = View.MoveCamera(Model.CurrentPanelType, input);
            
            if (!Model.NoSpritesLeft)
            {
                if (Model.IsWaitingForNode)
                {
                    if (View.CheckScrollNode(Model.CurrentNodeData, Model.CurrentPanelType))
                    {
                        Model.IsWaitingForNode = false;
                        Model.Update();
                        
                        var loadedNodeData = await _loadService.LoadNodeData(Model.CurrentNodeData);
                        var panelNode = _nodeFactory.Create(loadedNodeData);
                        panelNode.Activate(null);
                        
                        await CheckScrolling(input);
                    }
                }
                else if (Model.LoadNextNode())
                {
                    if (View.CheckScrollNode(Model.CurrentNodeData, Model.CurrentPanelType))
                    {
                        var loadedNodeData = await _loadService.LoadNodeData(Model.CurrentNodeData);
                        var nodeProcessor = _nodeFactory.Create(loadedNodeData);
                        nodeProcessor.Activate(null);
                        
                        await CheckScrolling(input);
                    }
                    else
                    {
                        Model.IsWaitingForNode = true;
                        Model.Update();
                    }
                }

                View.SetZoomGameSpritesTransparency(Model.CurrentPanelType, Model.GameSpritesDictionary.Values.ToList());
            }
            
            if (isScrollingEnded)
            {
                EndPanel();
            }
        }
        private async UniTask CheckClicking()
        {
            if(Model.CurrentPanelType != PanelType.Clicking)
            {
                return;
            }
            
            // TODO: implement this
            // if (input is over uiElement)
            // {
            //     return;
            // }
            
            Model.InstantNextMove = false;
            Model.IsBlocked = true;
            Model.Update();
            
            await CheckRecursive();
            
            async UniTask CheckRecursive()
            {
                var loadedNodeData = await _loadService.LoadNodeData(Model.CurrentNodeData);
                var nodeProcessor = _nodeFactory.Create(loadedNodeData);
                nodeProcessor.Activate(() =>
                {
                    if (Model.LoadNextNodeInStack())
                    {
                        CheckRecursive().Forget();
                    
                        return;
                    }
                
                    if (Model.LoadNextStack())
                    {
                        Model.IsBlocked = false;
                        Model.Update();
                    }
                });
            }
        }
        public void EndPanel()
        {
            if(Model.CurrentPanelType != PanelType.Clicking)
            {
                View.EndScrolling(Model.CurrentPanelType);
            }
            Model.Update();
            
            // _loadService.UnloadPanels();
            
            OnPanelEnd?.Invoke();
        }
        
        #endregion
    }
}