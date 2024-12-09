using System.Collections.Generic;
using System.Linq;
using Core.Base.Interfaces;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Utils;
using Core.Sprites.GameSprite;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Pools
{
    public class SpritePool : IBaseGenericPool<GameSpritePresenter>
    {
        private readonly Transform _parent;
        private readonly SpriteFactory _factory;
        private readonly Queue<GameSpritePresenter> _sprites = new();
        private readonly List<GameSpritePresenter> _activeSprites = new();

        [Inject]
        public SpritePool(SpriteFactory factory, Transform parent)
        {
            _parent = parent;
            _factory = factory;
        }

        public void Initialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Create();
            }
        }

        private GameSpritePresenter Create()
        {
            var spritePresenter = _factory.Create(_parent);
            spritePresenter.SetSpawned(false);
            spritePresenter.SetActive(false);
                
            spritePresenter.ReturnEvent += Return;

            _sprites.Enqueue(spritePresenter);
            
            return spritePresenter;
        }
        public GameSpritePresenter Spawn()
        {
            var spritePresenter = _sprites.Count > 0 ? _sprites.Dequeue() : Create();
            spritePresenter.SetSpawned(true);
            
            _activeSprites.Add(spritePresenter);
            
            return spritePresenter;
        }
        public void Return(GameSpritePresenter spritePresenter)
        {
            if(_sprites.Contains(spritePresenter))
            {
                this.LogWarning("You are trying to return already returned sprite");
                return;
            }

            spritePresenter.SetParent(_parent);
            spritePresenter.SetSpawned(false);
            spritePresenter.SetActive(false);
            
            _sprites.Enqueue(spritePresenter);
            _activeSprites.Remove(spritePresenter);
        }
        
        public void MoveActiveSprites(Vector3 moveVector)
        {
            foreach (var sprite in _activeSprites)
            {
                sprite.RepositionGameObject(moveVector);
            }
        }

        public GameSpritePresenter FindByTag(SpriteTag spriteTag)
        {
            return _activeSprites.FirstOrDefault(sprite => sprite.SpriteTag == spriteTag);
        }
        
        public void Clear()
        {
            foreach (var activeSprite in _activeSprites.ToArray())
            {
                Return(activeSprite);
            }
        }
    }
}