using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Infrastructure.Utils;
using Core.Node.Panel;
using Core.Node.Plot;
using Core.Sprites.Base;
using UniRx;

namespace Core.Game
{
    public class GameModel : BaseModel<GameModel>
    {
        public class DeferredEventArgs
        {
            private readonly DeferredEventType _deferredEventType;
            private event Action<Action> DeferredEvent;
            
            private int _count;
            private Action _onComplete;

            public DeferredEventArgs(DeferredEventType deferredEventType)
            {
                _deferredEventType = deferredEventType;
            }

            public void AddDeferredEvent(Action<Action> deferredEvent)
            {
                _count++;
                DeferredEvent += deferredEvent;
            }
            private void RemoveDeferredEvent()
            {
                _count--;

                if (_count == 0)
                {
                    _onComplete?.Invoke();
                }
            }

            public void StartDeferredEvent(Action onComplete)
            {
                _onComplete = onComplete;

                if (DeferredEvent != null)
                {
                    DeferredEvent.Invoke(RemoveDeferredEvent);
                    DeferredEvent = null;
                }
                else
                {
                    this.LogError("No deferred events of type " + _deferredEventType);
                }
                
            }
        }
        
        public NodeData CurrentNodeData => _currentStackNode?.nodeData[_currentNodeNumber];
        public bool IsBusy => IsPaused || IsBlocked;
        private bool IsLastNodeInStack => _currentNodeNumber >= _currentStackNode?.nodeData.Count - 1;
        
        public PanelType CurrentPanelType { get; private set; }
        
        public bool IsPaused;
        public bool IsBlocked;
        
        public bool FirstScroll;
        public bool NoSpritesLeft;
        public bool InstantNextMove;
        public bool IsWaitingForNode;
        
        private PlotContainer _currentPlot;
        private PanelData _currentPanelData;
        private PanelContainer _currentPanel;
        private StackNodeData _currentStackNode;
        private int _currentNodeNumber;

        public readonly ReactiveDictionary<NodeData, IGraphicsPresenter> GameSpritesDictionary = new();
        public readonly Dictionary<DeferredEventType, DeferredEventArgs> DeferredEvents = new();
        public readonly List<BaseService> Services = new();
        
        public readonly ReactiveDictionary<GameVariable, bool> GameVariables = new();
        public readonly ReactiveDictionary<ItemVariable, bool> ItemVariables = new();
        public readonly ReactiveDictionary<PosterPartVariable, bool> PosterPartVariables = new();
        
        public GameModel()
        {
            Subject = new BehaviorSubject<GameModel>(this);
            
            GameVariables.Clear();
            GameVariables.Clear();
            
            foreach (var variable in Enum.GetValues(typeof(PosterPartVariable)))
            {
                var castedVariable = (PosterPartVariable)variable;
                
                PosterPartVariables.Add(castedVariable, false);
            }
            
            foreach (var variable in Enum.GetValues(typeof(ItemVariable)))
            {
                var castedVariable = (ItemVariable)variable;
                
                ItemVariables.Add(castedVariable, false);
            }
            
            foreach (var variable in Enum.GetValues(typeof(GameVariable)))
            {
                var castedVariable = (GameVariable)variable;
                
                GameVariables.Add(castedVariable, false);
            }
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }

        public void SetPlot(PlotContainer plotContainer)
        {
            _currentPlot = plotContainer;
        }
        public void SetPanel(PanelData panelData)
        {
            if (panelData == null)
            {
                CurrentPanelType = PanelType.None;
                
                return;
            }
            
            CurrentPanelType = panelData.panelType;
            
            ClearCurrentPanelVariables();
        }
        
        public void SetLoadedPanel(LoadedPanelData loadedPanelData)
        {
            _currentPanel = loadedPanelData.PanelContainer;
            
            _currentStackNode = _currentPanel.stackNodeData.First(
                stack => stack.name == _currentPanel.nodeLinks.First(
                    node => node.baseNodeName == "START").targetNodeName);
        }

        public bool LoadNextCheck(string baseNodeName, bool value)
        {
            try
            {
                _currentStackNode = _currentPanel.stackNodeData.First(
                    stack => stack.name == _currentPanel.nodeLinks.FindAll(
                        node => node.baseNodeName == baseNodeName)[value ? 0 : 1].targetNodeName);
                _currentNodeNumber = 0;
                Update();

                return true;
            }
            catch (Exception)
            {
                NoSpritesLeft = true;
                Update();
                
                return false;
            }
        }
        public bool LoadNextChoice(string baseNodeName)
        {
            try
            {
                _currentStackNode = _currentPanel.stackNodeData.First(
                    stack => stack.name == _currentPanel.nodeLinks.First(
                        node => node.baseNodeName == baseNodeName).targetNodeName);
                _currentNodeNumber = 0;
                Update();

                return true;
            }
            catch (Exception)
            {
                NoSpritesLeft = true;
                Update();
                
                return false;
            }
        }
        public bool LoadNextNode()
        {
            var result = true;
            
            if (IsLastNodeInStack)
            {
                result = LoadNextStack();
            }
            else
            {
                _currentNodeNumber++;
                Update();
            }

            return result;
        }
        public bool LoadNextNodeInStack()
        {
            if (IsLastNodeInStack)
            {
                return false;
            }

            _currentNodeNumber++;
            Update();
            
            return true;
        }
        public bool LoadNextStack()
        {
            try
            {
                _currentStackNode = _currentPanel.stackNodeData.First(
                    stack => stack.name == _currentPanel.nodeLinks.First(
                        node => node.baseNodeName == _currentStackNode.name).targetNodeName);
                _currentNodeNumber = 0;
                Update();

                return true;
            }
            catch (Exception)
            {
                NoSpritesLeft = true;
                Update();
                
                return false;
            }
        }
        
        public bool LoadSpecificPanel(string name, out PanelData nextPanel)
        {
            try
            {
                nextPanel = _currentPlot.panelNodeData.First(
                    panel => panel.id == name);
                
                _currentPanelData = nextPanel;
            }
            catch (Exception)
            {
                this.LogError("No panel " + name + " in plot");
                nextPanel = null;
                return false;
            }

            return true;
        }
        public bool LoadNextPanel(out PanelData nextPanel)
        {
            try
            {
                if (_currentPanelData.hasCondition)
                {
                    bool condition = GameVariables[_currentPanelData.plotCondition];

                    if (condition)
                    {
                        nextPanel = _currentPlot.panelNodeData.First(
                            panel => panel.id == _currentPlot.nodeLinks.First(
                                otherPanel => otherPanel.baseNodeName == _currentPanelData.id).targetNodeName);
                    }
                    else
                    {
                        nextPanel = _currentPlot.panelNodeData.First(
                            panel => panel.id == _currentPlot.nodeLinks.Last(
                                otherPanel => otherPanel.baseNodeName == _currentPanelData.id).targetNodeName);
                    }
                }
                else
                {
                    nextPanel = _currentPlot.panelNodeData.First(
                        panel => panel.id == _currentPlot.nodeLinks.First(
                            otherPanel => otherPanel.baseNodeName == _currentPanelData.id).targetNodeName);
                }

                _currentPanelData = nextPanel;
            }
            catch (Exception)
            {
                if (_currentPanelData.hasCondition)
                {
                    this.LogError("No next panel in plot with condition " + _currentPanelData.plotCondition);
                }
                else
                {
                    this.LogError("No next panel in plot");
                }
                nextPanel = null;
                return false;
            }

            return true;
        }
        
        public void StartDeferredEvent(DeferredEventType deferredEventType, Action onComplete)
        {
            if (DeferredEvents.TryGetValue(deferredEventType, out var eventArgs))
            {
                eventArgs.StartDeferredEvent(onComplete);
            }
            else if (deferredEventType != DeferredEventType.ScrollStarted)
            {
                this.LogError("No deferred events of type " + deferredEventType);
            }
        }
        public void AddToDeferredEvent(DeferredEventType deferredEventType, Action<Action> deferredAction)
        {
            DeferredEvents.TryAdd(deferredEventType, new DeferredEventArgs(deferredEventType));
            DeferredEvents[deferredEventType].AddDeferredEvent(deferredAction);
            
            Update();
        }

        public void ChangeGlobalVariable(Type globalVariableType, int globalVariableIndex, bool value)
        {
            switch (globalVariableType) {
                case { } variableType when variableType == typeof(GameVariable):
                {
                    var variable = (GameVariable)globalVariableIndex;
            
                    GameVariables[variable] = value;
                    
                    break;
                }
                case { } variableType when variableType == typeof(ItemVariable):
                {
                    var variable = (ItemVariable)globalVariableIndex;
            
                    ItemVariables[variable] = value;
                    
                    break;
                }
                case { } variableType when variableType == typeof(PosterPartVariable):
                {
                    var variable = (PosterPartVariable)globalVariableIndex;
            
                    PosterPartVariables[variable] = value;
                    
                    break;
                }
                default:
                {
                    this.LogError("Unknown variable type " + globalVariableType);
                    
                    break;
                }
            }
        }

        private void ClearCurrentPanelVariables()
        {
            _currentNodeNumber = 0;

            FirstScroll = false;
            NoSpritesLeft = false;
            InstantNextMove = false;
            IsWaitingForNode = CurrentPanelType == PanelType.Vertical;
        }

        public void Clear()
        {
            GameSpritesDictionary.Clear();
            
            foreach (var variable in Enum.GetValues(typeof(ItemVariable)))
            {
                var castedVariable = (ItemVariable)variable;
                
                ItemVariables[castedVariable] = false;
            }
            
            foreach (var variable in Enum.GetValues(typeof(GameVariable)))
            {
                var castedVariable = (GameVariable)variable;
                
                GameVariables[castedVariable] = false;
            }
            
            foreach (var variable in Enum.GetValues(typeof(PosterPartVariable)))
            {
                var castedVariable = (PosterPartVariable)variable;
                
                PosterPartVariables[castedVariable] = false;
            }
        }
    }
}