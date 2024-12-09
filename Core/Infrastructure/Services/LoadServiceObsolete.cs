// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Core.Base.Classes;
// using Core.Engine;
// using Core.Mono;
// using Cysharp.Threading.Tasks;
// using Node.Panel;
// using Node.Plot;
// using UnityEngine;
//
// namespace Core.Infrastructure.Services
// {
//     public class LoadServiceObsolete
//     {
//         #region FIELDS
//
//         private PlotLoader _plotLoader;
//         private PanelLoader _panelLoader;
//
//         //private readonly List<PanelContainer> _panels = new List<PanelContainer>();
//         private PanelNodeData _currentPanelData;
//         private PanelContainer _currentPanel;
//         private StackNodeData _currentFrame;
//         
//         private PlotContainer _plotContainer;
//         private readonly List<PanelPrefab> _previousPanelPrefabs = new();
//         private PanelPrefab _currentPanelPrefab;
//         private readonly List<GameObject> _previousContentPrefabs = new();
//         private GameObject _currentContentPrefab;
//         
//         private PanelType _currentPanelType;
//         private bool _noSpritesLeft;
//         private int _currentMove;
//
//         #endregion
//         #region PROPERTIES
//
//         public PanelType CurrentPanelType => _currentPanelType;
//         public bool IsCurrentPanelVertical => _currentPanelType == PanelType.Vertical;
//         public bool NoMovesLeft => _currentMove < _currentFrame?.nodeData.Count;
//         public bool IsLastMove => _currentMove >= _currentFrame?.nodeData.Count - 1;
//         public NodeData CurrentMoveNodeData => _currentFrame?.nodeData[_currentMove];
//         public Transform CurrentContentPrefabTransform => _currentContentPrefab == null ? null : _currentContentPrefab.transform;
//
//         #endregion
//
//         #region MAIN_FUNCTIONS
//
//         public async UniTask Initialize()
//         {
//             // foreach (var panel in _plotContainer.panelNodeData)
//             // {
//             //     if(panel.name == "START")
//             //         continue;
//             //     _panels.Add(Resources.Load<PanelContainer>("Containers/" + panel.panelName));
//             // }
//             
//             _plotLoader = new PlotLoader();
//             _panelLoader = new PanelLoader();
//             
//             _plotContainer = await _plotLoader.LoadAsync();
//         }
//         private async void LoadNextPanel()
//         {
//             await LoadPanel();
//         }
//         private async UniTask LoadAsync()
//         {
//             _currentPanelData = _plotContainer.panelNodeData.First(x => x.name == "START");
//             CheckLevelToLoad();
//             await LoadPanel();
//         }
//         private async UniTask LoadByName(string panelName)
//         {
//             _currentPanelData = _plotContainer.panelNodeData.First(x => x.panelName == panelName);
//             CheckLevelToLoad();
//             await LoadPanel(false);
//         }
//         public void SaveData()
//         {
//             
//         }
//         public void LoadData()
//         {
//             
//         }
//
//         #endregion
//         #region VARIABLE_FUNCTIONS
//
//         public void ChangeVariable(GameVariable gameVariable, bool value)
//         {
//             // _variables.Remove(gameVariable);
//             // _variables.Add(gameVariable, value);
//         }
//         private bool CheckVariable(GameVariable gameVariable)
//         {
//             // _variables.TryGetValue(gameVariable, out var value);
//
//             // return value;
//             return true;
//         }
//
//         #endregion
//         #region PANEL_LOADING
//         
//         private void CheckLevelToLoad()
//         {
//             // var level = -1;
//             //
//             // if(PlayerPrefs.HasKey("level"))
//             // {
//             //     level = PlayerPrefs.GetInt("level");
//             // }
//         }
//         private async UniTask LoadPanel(bool next = true)
//         {
//             // Main.Instance.SetUpdateBlocked(true);
//             
//             // Calculating next panel name
//             // By default we are searching for the next panel by the link from current (we suppose that current panel is already in use)
//             // But also we can load current panel if it's not loaded
//             //Debug.Log(CheckNextPanel());
//             if (next)
//                 if(CheckNextPanel())
//                     return;
//             
//             // _currentPanel = _panels.First(x => x.panelName == _currentPanelData.panelName);
//             
//             // _currentPanel = await _panelLoader.LoadInternal($"{_currentPanelData.panelName}");
//             // if (_currentPanel == null)
//             // {
//             //     Debug.LogError($"NO SUCH PANEL AS {_currentPanelData.panelName}!!!");
//             //     return;
//             // }
//             
//             // Reset variables
//             _noSpritesLeft = false;
//             
//             // Clear Main
//             _currentPanelType = _currentPanelData.panelType;
//             // MainFolder.Main.Instance.ClearBeforeNextPanel(_currentPanelType == PanelType.Clicking);
//
//             // Set new panel
//             _currentMove = 0;
//             
//             _currentFrame = _currentPanel.stackNodeData.First(
//                 x => x.name == _currentPanel.nodeLinks.First(y =>
//                     y.baseNodeName == "START").targetNodeName);
//             
//             // Sound
//             if (_currentPanelData.hasAudio)
//             {
//                 // MainFolder.Main.Instance.SoundManager.SetMusic(_currentPanelData.audio);
//             }
//
//             // Prefab background
//             // if (_currentPanelData.hasPrefab)
//             // {
//             //     var panelPrefab = Instantiate(_currentPanelData.prefab, backgroundPool);
//             //     if(_currentPanelPrefab != null)
//             //     {
//             //         _previousPanelPrefabs.Add(_currentPanelPrefab);
//             //     }
//             //     _currentPanelPrefab = panelPrefab.GetComponent<PanelPrefab>();
//             //     _currentPanelPrefab.SetPanelType(_currentPanelType);
//             //
//             //     if (_currentContentPrefab != null)
//             //     {
//             //         _previousContentPrefabs.Add(_currentContentPrefab);
//             //         _currentContentPrefab = null;
//             //     }
//             //     
//             //     if (_currentPanelPrefab.ContentPrefab != null)
//             //     {
//             //         _currentContentPrefab = Instantiate(_currentPanelPrefab.ContentPrefab, contentPool);
//             //         // Main.Instance.SpawnManager.SetStage(_currentContentPrefab, _currentPanelPrefab.Stage);
//             //     }
//             //
//             //     if (_currentPanelPrefab.GetComponent<CinemachinePath>() is var path && path != null)
//             //     {
//             //         // Main.Instance.Waypoints = path.m_Waypoints;
//             //     }
//             // }
//             // CheckBounds();
//             
//             // Main.Instance.SetUpdateBlocked(false);
//             
//             bool CheckNextPanel()
//             {
//                 try
//                 {
//                     PanelNodeData nextPanel;
//                     if (_currentPanelData.hasCondition)
//                     {
//                         bool condition = false;
//
//                         switch (_currentPanelData.plotCondition)
//                         {
//                             case PlotCondition.IsGoodEnding:
//                             {
//                                 condition = false;
//                                 break;
//                             }
//                             case PlotCondition.IsBadEnding:
//                             {
//                                 condition = false;
//                                 break;
//                             }
//                             case PlotCondition.Is4PanelLeft:
//                             {
//                                 condition = CheckVariable(GameVariable.Is4PanelLeft);
//                                 break;
//                             }
//                     
//                         }
//
//                         if (condition)
//                         {
//                             nextPanel = _plotContainer.panelNodeData.First(
//                                 x => x.name == _plotContainer.nodeLinks.First(y =>
//                                     y.baseNodeName == _currentPanelData.name).targetNodeName);
//                         }
//                         else
//                         {
//                             nextPanel = _plotContainer.panelNodeData.First(
//                                 x => x.name == _plotContainer.nodeLinks.Last(y =>
//                                     y.baseNodeName == _currentPanelData.name).targetNodeName);
//                         }
//                     }
//                     else
//                     {
//                         nextPanel = _plotContainer.panelNodeData.First(
//                             x => x.name == _plotContainer.nodeLinks.First(y =>
//                                 y.baseNodeName == _currentPanelData.name).targetNodeName);
//                     }
//                     _currentPanelData = nextPanel;
//                 }
//                 catch (Exception)
//                 {
//                     return true;
//                 }
//
//                 return false;
//             }
//         }
//         private bool NextFrame()
//         {
//             try
//             {
//                 _currentFrame = _currentPanel.stackNodeData.First(
//                     x => x.name == _currentPanel.nodeLinks.First(y =>
//                         y.baseNodeName == _currentFrame.name).targetNodeName);
//                 _currentMove = 0;
//
//                 return true;
//             }
//             catch (Exception)
//             {
//                 _noSpritesLeft = true;
//                 return false;
//             }
//         }
//         private bool NextMove()
//         {
//             if (_currentMove >= _currentFrame.nodeData.Count - 1)
//             {
//                 NextFrame();
//                 return false;
//             }
//             
//             ChangeCurrentMove(true);
//             return true;
//         }
//
//         #endregion
//         #region SPECIAL_FUNCTIONS
//
//         private void ChangeCurrentMove(bool increase)
//         {
//             if(increase)
//             {
//                 _currentMove++;
//             }
//             else
//             {
//                 _currentMove = 0;
//             }
//         }
//         private void ChangeCurrentFrame(StackNodeData frameStackNodeData)
//         {
//             _currentFrame = frameStackNodeData;
//         }
//         private StackNodeData CheckNextFrameAfterChoice()
//         {
//             return _currentPanel.stackNodeData.First(
//                 x => x.name == _currentPanel.nodeLinks.First(y =>
//                     y.baseNodeName == _currentFrame.nodeData[_currentMove].name).targetNodeName);
//         }
//         private void CheckNextAfterVariable(GameVariable gameVariable)
//         {
//             if(CheckVariable(gameVariable))
//             {
//                 _currentFrame = _currentPanel.stackNodeData.First(
//                     x => x.name == _currentPanel.nodeLinks.First(y =>
//                         y.baseNodeName == _currentFrame.nodeData[_currentMove].name).targetNodeName);
//             }
//             else
//             {
//                 _currentFrame = _currentPanel.stackNodeData.First(
//                     x => x.name == _currentPanel.nodeLinks.Last(y =>
//                         y.baseNodeName == _currentFrame.nodeData[_currentMove].name).targetNodeName);
//             }
//             
//             _currentMove = 0;
//         }
//         private void CheckNextAfterPencil(GameVariable gameVariable)
//         {
//             if(_currentFrame.nodeData[_currentMove].gameVariable == gameVariable)
//             {
//                 _currentFrame = _currentPanel.stackNodeData.First(
//                     x => x.name == _currentPanel.nodeLinks.First(y =>
//                         y.baseNodeName == _currentFrame.nodeData[_currentMove].name).targetNodeName);
//             }
//             else
//             {
//                 _currentFrame = _currentPanel.stackNodeData.First(
//                     x => x.name == _currentPanel.nodeLinks.Last(y =>
//                         y.baseNodeName == _currentFrame.nodeData[_currentMove].name).targetNodeName);
//             }
//             _currentMove = 0;
//         }
//         private void DeletePreviousPanelPrefab()
//         {
//             if (_previousPanelPrefabs.Count != 0 && _currentPanelData.deletePreviousPanel)
//             {
//                 foreach (var previousPanelPrefab in _previousPanelPrefabs)
//                 {
//                     UnityEngine.Object.Destroy(previousPanelPrefab.gameObject);
//                 }
//                 _previousPanelPrefabs.Clear();
//                 
//                 foreach (var previousContentPrefab in _previousContentPrefabs)
//                 {
//                     UnityEngine.Object.Destroy(previousContentPrefab.gameObject);
//                 }
//                 _previousContentPrefabs.Clear();
//                 
//                 //_panelLoader.UnloadInternal();
//             }
//         }
//         private void DeleteAllPanelPrefab()
//         {
//             if (_currentPanelPrefab == null)
//                 return;
//             
//             _previousPanelPrefabs.Add(_currentPanelPrefab);
//             foreach (var previousPanelPrefab in _previousPanelPrefabs)
//             {
//                 UnityEngine.Object.Destroy(previousPanelPrefab.gameObject);
//             }
//             _previousPanelPrefabs.Clear();
//         }
//         private void DeleteAllContentPrefab()
//         {
//             if(_currentContentPrefab != null)
//             {
//                 _previousContentPrefabs.Add(_currentContentPrefab);
//             }
//             
//             foreach (var previousContentPrefab in _previousContentPrefabs)
//             {
//                 UnityEngine.Object.Destroy(previousContentPrefab.gameObject);
//             }
//             _previousContentPrefabs.Clear();
//             
//             _panelLoader.UnloadAll();
//         }
//
//         private void ResetVariables()
//         {
//             // _variables.Clear();
//             // foreach (var value in Enum.GetValues(typeof(GameVariable)))
//             // {
//             //     _variables.Add((GameVariable)value, false);
//             // }
//             //
//             
//         }
//         private void MovePanelPrefab(Vector3 moveVector)
//         {
//             foreach (var previousPanelPrefab in _previousPanelPrefabs)
//             {
//                 previousPanelPrefab.transform.position += moveVector;
//             }
//
//             _currentPanelPrefab.transform.position += moveVector;
//         }
//         private bool CheckLastPanel()
//         {
//             Debug.Log(_currentPanel.name);
//             return _currentPanel.name == "Panel_13";
//         }
//         
//         #endregion
//
//     }
// }