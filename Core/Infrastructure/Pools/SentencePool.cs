using System;
using System.Collections.Generic;
using Core.Base.Interfaces;
using Core.Infrastructure.Factories;
using UI.HUD.BacklogMenu;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Pools
{
    public class SentencePool : IBaseGenericPool<SentenceMono>
    {
        public event Action<int> OnCountChanged;
        
        private readonly Transform _parent;
        private readonly SentenceFactory _factory;
        private readonly Queue<SentenceMono> _sentences = new();
        private readonly List<SentenceMono> _activeSentences = new();

        [Inject]
        public SentencePool(SentenceFactory factory, Transform parent)
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

        private SentenceMono Create()
        {
            var sentence = _factory.Create(_parent);
            sentence.gameObject.SetActive(false);
            sentence.OnReturn += Return;

            _sentences.Enqueue(sentence);
            
            return sentence;
        }
        public SentenceMono Spawn()
        {
            var spritePresenter = _sentences.Count > 0 ? _sentences.Dequeue() : Create();
            spritePresenter.gameObject.SetActive(true);
            
            _activeSentences.Add(spritePresenter);
            
            OnCountChanged?.Invoke(_activeSentences.Count);
            
            return spritePresenter;
        }
        public void Return(SentenceMono sentence)
        {
            if(_sentences.Contains(sentence))
            {
                Debug.LogError("You are trying to return already returned sentence");
                return;
            }
            
            sentence.gameObject.SetActive(false);
            sentence.Reset();
            
            _sentences.Enqueue(sentence);
            _activeSentences.Remove(sentence);
            
            OnCountChanged?.Invoke(_activeSentences.Count);
        }
        
        public void Clear()
        {
            foreach (var activeSprite in _activeSentences.ToArray())
            {
                Return(activeSprite);
            }
            
            OnCountChanged?.Invoke(_activeSentences.Count);
        }
    }
}