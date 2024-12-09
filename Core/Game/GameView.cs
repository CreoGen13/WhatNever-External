using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Pools;
using Core.Infrastructure.Utils;
using Core.Mono;
using Core.Node.Panel;
using Core.Sprites.Base;
using Core.Sprites.GameBubble;
using Core.Sprites.GameSprite;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using Zenject;

namespace Core.Game
{
    public class GameView : BaseView
    {
        #region EVENTS

        public event Action OnAwake;
        public event Action OnUpdate;
        public event Action OnExit;

        #endregion
        #region ASSETS
        
        [Header("Assets")]
        [SerializeField] private GameObject content;
        [SerializeField] private Canvas backgroundPool;
        [SerializeField] private Canvas [] contentStages;
        [SerializeField] private CinemachinePath cinemachinePath;
        [SerializeField] private Transform backScreenBlock;
        
        #endregion
        #region FIELDS
        
        private readonly List<PanelPrefab> _previousPanelPrefabs = new();
        private readonly List<GameObject> _previousContentPrefabs = new();
        private PanelPrefab _currentPanelPrefab;
        private GameObject _currentContentPrefab;
        private BaseMechanics _currentMechanicsGameObject;
        private Transition _currentTransition;
        
        private const float OPTIMAL_HEIGHT = 1080f;
        private const float OPTIMAL_WIDTH = 1920f;
        private const float OPTIMAL_RATIO = OPTIMAL_WIDTH / OPTIMAL_HEIGHT;
        private float _scrollingPosition;
        private float _lowerBound;

        private Sequence _cameraSequence;

        public CinemachinePath.Waypoint[] Waypoints
        {
            set => cinemachinePath.m_Waypoints = value;
        } 

        #endregion
        #region INJECTIONS

        private ScriptableGameSettings _gameSettings;
        
        private SpritePool _spritePool;
        private BubblePool _bubblePool;
        private MechanicsFactory _mechanicsFactory;
        private MonoFactory _monoFactory;
        
        private Camera _mainCamera;

        #endregion
        #region MAIN

        [Inject]
        private void Construct(
            Camera mainCamera,
            SpritePool spritePool,
            BubblePool bubblePool,
            MechanicsFactory mechanicsFactory,
            MonoFactory monoFactory,
            ScriptableGameSettings gameSettings)
        {
            _mainCamera = mainCamera;
            
            _spritePool = spritePool;
            _bubblePool = bubblePool;
            _mechanicsFactory = mechanicsFactory;
            _monoFactory = monoFactory;
            
            _gameSettings = gameSettings;
            
            _spritePool.Initialize(20);
        }

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnExit?.Invoke();
        }

        #endregion
        #region GAME LOGICS

        public void ManualUpdate()
        {
            if (_currentMechanicsGameObject)
            {
                _currentMechanicsGameObject.ManualUpdate();
            }
            
            _currentPanelPrefab?.UpdateFadeHelpers(_mainCamera.transform.position.z);
        }
        
        public bool MoveCamera(PanelType panelType, Vector2 input)
        {
            bool isScrollingEnded = false;
            
            switch (panelType)
            {
                case PanelType.Vertical:
                {
                    var currentPosition = content.transform.position;
                    var changeValue = input.y * (_gameSettings.scrollingSpeed * _gameSettings.scrollAdditionalScale);

                    if (backScreenBlock.position.y + changeValue < 0)
                    {
                        changeValue = -backScreenBlock.position.y;
                    }
                    
                    var position = new Vector3(currentPosition.x, currentPosition.y + changeValue, currentPosition.z);
                    
                    position.y = position.y >= _lowerBound - _mainCamera.orthographicSize ? new Func<float>(() =>
                    {
                        isScrollingEnded = true;
                        return _lowerBound - _mainCamera.orthographicSize;

                    })() : position.y;
                    content.transform.position = position;
                    
                    break;
                }
                case PanelType.Horizontal:
                {
                    var currentPosition = content.transform.position;
                    var changeValue = input.x * (_gameSettings.scrollingSpeed * _gameSettings.scrollAdditionalScale);

                    if (backScreenBlock.position.x + changeValue > 0)
                    {
                        changeValue = backScreenBlock.position.x;
                    }
                    //TODO: refactor this, there is bug
                    
                    Vector3 position = new Vector3(currentPosition.x + changeValue, currentPosition.y, currentPosition.z);

                    position.x = position.x <= _lowerBound + _mainCamera.orthographicSize * OPTIMAL_RATIO ? new Func<float>(() =>
                    {
                        isScrollingEnded = true;
                        return _lowerBound + _mainCamera.orthographicSize * OPTIMAL_RATIO;

                    })() : position.x;
                    content.transform.position = position;
                    
                    break;
                }
                case PanelType.Zoom:
                {
                    var changeValue = input.y * (_gameSettings.scrollingSpeed * _gameSettings.scrollAdditionalScale);
                    _scrollingPosition = Mathf.Max(0, _scrollingPosition - changeValue);
                    content.transform.position = -cinemachinePath.EvaluatePositionAtUnit(_scrollingPosition, CinemachinePathBase.PositionUnits.Distance);
                    if(cinemachinePath.PathLength <= _scrollingPosition)
                    {
                        isScrollingEnded = true;
                    }
                    
                    break;
                }
            }

            return isScrollingEnded;
        }
        
        public bool CheckScrollNode(NodeData nodeData, PanelType currentPanelType)
        {
            if (nodeData.nextNodeConnection == NextNodeConnection.WithNextNode ||
                currentPanelType == PanelType.Vertical && content.transform.localPosition.y + Screen.height / 2f >= -nodeData.spritePosition.y ||
                currentPanelType == PanelType.Horizontal && content.transform.localPosition.x - Screen.width / 2f <= -nodeData.spritePosition.x ||
                currentPanelType == PanelType.Zoom && -content.transform.localPosition.z >= nodeData.spritePosition.z)
            {
                return true;
            }
        
            return false;
        }
        public void EndScrolling(PanelType currentPanelType)
        {
            _scrollingPosition = 0f;
            content.transform.position = Vector3.zero;
            
            var moveVector = currentPanelType switch
            {
                PanelType.Horizontal => new Vector3(_lowerBound + _mainCamera.orthographicSize * OPTIMAL_RATIO, 0, 0),
                PanelType.Vertical => new Vector3(0, _lowerBound - _mainCamera.orthographicSize, 0),
                PanelType.Zoom when cinemachinePath.m_Waypoints.Any() => -cinemachinePath.EvaluatePositionAtUnit(1, CinemachinePathBase.PositionUnits.Normalized),
                PanelType.Zoom => new Vector3(0, 0, _lowerBound),
                _ => throw new ArgumentException()
            };

            backScreenBlock.position = Vector3.zero;
            if (_currentPanelPrefab)
            {
                _currentPanelPrefab.transform.position += moveVector;
            }
            if(_currentContentPrefab)
            {
                _currentContentPrefab.transform.position += moveVector;
            }
            if (_currentMechanicsGameObject)
            {
                _currentMechanicsGameObject.transform.position += moveVector;
            }
            
            _spritePool.MoveActiveSprites(moveVector);
            _bubblePool.MoveActiveBubbles(moveVector);
        }
        
        public void SpawnPanelPrefab(GameObject prefab, PanelType currentPanelType)
        {
            if (_currentPanelPrefab != null)
            {
                _previousPanelPrefabs.Add(_currentPanelPrefab);
                _currentPanelPrefab = null;
            }
            
            _currentPanelPrefab = Instantiate(prefab, backgroundPool.transform).GetComponent<PanelPrefab>();
            _currentPanelPrefab.Init(currentPanelType);
            _currentPanelPrefab.InitFadeHelpers(_gameSettings.zoomSpritesTransparencyStartDistance, _gameSettings.zoomSpritesTransparencyEndDistance);
            
            if (_currentContentPrefab != null)
            {
                _previousContentPrefabs.Add(_currentContentPrefab);
                _currentContentPrefab = null;
            }
                
            if (_currentPanelPrefab.ContentPrefab != null)
            {
                _currentContentPrefab = Instantiate(_currentPanelPrefab.ContentPrefab, contentStages[GameUtil.GetStage(_currentPanelPrefab.Stage)].transform);
            }
            
            if (_currentPanelPrefab.GetComponent<CinemachinePath>() is var path && path != null)
            {
                Waypoints = path.m_Waypoints;
            }

            _lowerBound = _currentPanelPrefab.GetLowerBound();
        }
        public T SpawnMechanics<T>(GameObject prefab, Stage stage) where T : BaseMechanics
        {
            if (_currentMechanicsGameObject)
            {
                this.LogError("You are spawning new mechanics before completing previous of type " + _currentMechanicsGameObject.GetType());
            }
            
            var mechanics = _mechanicsFactory.Create<T>(new MechanicsFactory.Data
            {
                Prefab = prefab,
                Parent = contentStages[GameUtil.GetStage(stage)].transform
            });

            if (mechanics == null)
            {
                this.LogError("GameObject of mechanics doesn't contain it's script");

                return null;
            }

            mechanics.DestroyEvent += OnMechanicsDestroy;

            _currentMechanicsGameObject = mechanics;
            
            return (T) _currentMechanicsGameObject;
        }
        public T SpawnMono<T>(GameObject prefab, Stage stage) where T : MonoBehaviour
        {
            var monoBehaviour = _monoFactory.Create<T>(new MonoFactory.Data
            {
                Prefab = prefab,
                Parent = contentStages[GameUtil.GetStage(stage)].transform
            });

            if (monoBehaviour == null)
            {
                this.LogError("GameObject of mechanics doesn't contain it's script");
            }
            
            return monoBehaviour;
        }
        private void OnMechanicsDestroy(BaseMechanics mechanics)
        {
            mechanics.DestroyEvent -= OnMechanicsDestroy;
                
            if(_currentMechanicsGameObject && _currentMechanicsGameObject == mechanics)
            {
                _currentMechanicsGameObject = null;
            }
        }
        
        public GameSpritePresenter SpawnSprite(SpriteTag spriteTag, Stage stage)
        {
            GameSpritePresenter sprite;
            if (spriteTag != SpriteTag.New)
            {
                sprite = _spritePool.FindByTag(spriteTag);
                
                if (sprite != null)
                {
                    sprite.SetActive(false);
                    return sprite;
                }
            }
            
            sprite = _spritePool.Spawn();
            sprite.SetParent(contentStages[GameUtil.GetStage(stage)].transform);
            
            return sprite;
        }
        public GameBubblePresenter SpawnBubble(GameBubbleView prefab, Stage stage)
        {
            var bubble = _bubblePool.Spawn(prefab);
            bubble.SetParent(contentStages[GameUtil.GetStage(stage)].transform);
            
            return bubble;
        }
        
        public void PlayZoom(Vector3 position, float zoom, Action onComplete = null, bool instant = false)
        {
            var endPosition = new Vector3(position.x, position.y, position.z - _mainCamera.fieldOfView);
            
            if (Mathf.Approximately(_mainCamera.orthographicSize, _gameSettings.cameraDefaultSize / zoom) ||
                _mainCamera.transform.position == endPosition)
            {
                return;
            }

            if (instant)
            {
                _mainCamera.orthographicSize = _gameSettings.cameraDefaultSize / zoom;
                _mainCamera.transform.position = endPosition;
                    
                onComplete?.Invoke();
            }
            
            _cameraSequence?.Kill();
            _cameraSequence = DOTween.Sequence();
            _cameraSequence.Join(DOTween.To(size => _mainCamera.orthographicSize = size, _mainCamera.orthographicSize, _gameSettings.cameraDefaultSize / zoom, _gameSettings.cameraAnimationDuration));
            _cameraSequence.Join(DOTween.To(() => _mainCamera.transform.position, pos => _mainCamera.transform.position = pos, endPosition, _gameSettings.cameraAnimationDuration));
            _cameraSequence.OnComplete( () => {
                onComplete?.Invoke();
            });
            _cameraSequence.Play();
        }
        public void PlayTransition(Transition transition, Action onComplete)
        {
            _currentTransition = transition;
            _currentTransition.Play(onComplete);
        }
        public void StopTransition(Action onComplete)
        {
            if (_currentTransition)
            {
                _currentTransition.Stop(onComplete);
                _currentTransition = null;
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        
        public void DestroyPanels()
        {
            foreach (var previousPanelPrefab in _previousPanelPrefabs)
            {
                this.Log("Panel prefab " + previousPanelPrefab.gameObject.name + " is Deleted");
                
                Destroy(previousPanelPrefab.gameObject);
            }
            
            _previousPanelPrefabs.Clear();
            
            foreach (var previousContentPrefab in _previousContentPrefabs)
            {
                this.Log("Content prefab " + previousContentPrefab.gameObject.name + " is Deleted");
                
                Destroy(previousContentPrefab);
            }
            
            _previousContentPrefabs.Clear();
        }
        public void Clear()
        {
            Destroy(_currentPanelPrefab.gameObject);
            
            _currentPanelPrefab = null;
            
            Destroy(_currentContentPrefab);
            
            _currentPanelPrefab = null;

            DestroyPanels();

            if (_currentMechanicsGameObject)
            {
                _currentMechanicsGameObject.Destroy();
            }
            
            content.transform.position = Vector3.zero;
            _scrollingPosition = 0;
        }
        
        public void SetZoomGameSpritesTransparency(PanelType panelType, List<IGraphicsPresenter> gameSpritePresenters)
        {
            if (panelType != PanelType.Zoom)
            {
                return;
            }
            
            if (gameSpritePresenters == null)
            {
                return;
            }
            
            foreach (var gameSpritePresenter in gameSpritePresenters)
            {
                var distance = Mathf.Abs(gameSpritePresenter.Transform.position.z - _mainCamera.transform.position.z);
                
                if (distance <= _gameSettings.zoomSpritesTransparencyStartDistance)
                {
                    var alpha = distance / _gameSettings.zoomSpritesTransparencyStartDistance;

                    gameSpritePresenter.SetTransparency(alpha);
                }
            }
        }

        #endregion
    }
}