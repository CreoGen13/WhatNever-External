using System.Collections.Generic;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Utils;
using Core.Sprites.GameBubble;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Pools
{
    public class BubblePool
    {
        private readonly Transform _parent;
        private readonly BubbleFactory _factory;
        private readonly Dictionary<string, Queue<GameBubblePresenter>> _bubblesByPrefab = new();
        private readonly List<GameBubblePresenter> _activeBubbles = new();

        [Inject]
        public BubblePool(BubbleFactory factory, Transform parent)
        {
            _parent = parent;
            _factory = factory;
        }

        private GameBubblePresenter Create(GameBubbleView prefab)
        {
            var bubblePresenter = _factory.Create(prefab, _parent);
            bubblePresenter.SetSpawned(false);
            bubblePresenter.SetActive(false);
                
            bubblePresenter.ReturnEvent += Return;

            _activeBubbles.Add(bubblePresenter);
            
            return bubblePresenter;
        }
        public GameBubblePresenter Spawn(GameBubbleView prefab)
        {
            GameBubblePresenter bubblePresenter;
            
            if (_bubblesByPrefab.TryGetValue(prefab.gameObject.name, out var bubbles) && bubbles.Count > 0)
            {
                bubblePresenter = bubbles.Dequeue();
            }
            else
            {
                bubblePresenter = Create(prefab);
                
                _bubblesByPrefab.TryAdd(prefab.gameObject.name, new Queue<GameBubblePresenter>());
            }
             
            bubblePresenter.SetSpawned(true);
            
            _activeBubbles.Add(bubblePresenter);
            
            return bubblePresenter;
        }
        public void Return(GameBubblePresenter bubblePresenter)
        {
            if(!_bubblesByPrefab.TryGetValue(bubblePresenter.PrefabName, out var bubbles))
            {
                this.LogWarning("You are trying to return sprite to pool that does not exist");
                
                return;
            }

            if (bubbles.Contains(bubblePresenter))
            {
                this.LogWarning("You are trying to return already returned sprite");
                
                return;
            }

            bubblePresenter.SetParent(_parent);
            bubblePresenter.SetSpawned(false);
            bubblePresenter.SetActive(false);
            
            bubbles.Enqueue(bubblePresenter);
            _activeBubbles.Remove(bubblePresenter);
        }
        
        public void MoveActiveBubbles(Vector3 moveVector)
        {
            foreach (var bubble in _activeBubbles)
            {
                bubble.RepositionGameObject(moveVector);
            }
        }
        
        public void Clear()
        {
            foreach (var activeBubble in _activeBubbles.ToArray())
            {
                Return(activeBubble);
            }
        }
    }
}