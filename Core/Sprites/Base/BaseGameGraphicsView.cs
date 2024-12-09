using System;
using Core.Base.Classes;
using Core.Infrastructure.Enums;
using DG.Tweening;
using Scriptables.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Sprites.Base
{
    public abstract class BaseGameGraphicsView : BaseView
    {
        [Header("Main")]
        [SerializeField] protected Image image;
        [SerializeField] protected RectTransform rectTransform;
        [SerializeField] protected AudioSource audioSource;
        
        protected const string EntryName = "Entry";

        protected ScriptableGameSettings GameSettings;
        
        [Inject]
        private void Construct(ScriptableGameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }
        
        public virtual void SetTransform(Vector3 position, Vector3 scaleAndRotation, Stage stage = Stage.Third)
        {
            var yPosition = stage switch
            {
                Stage.Fifth => -40,
                Stage.Fourth => -30,
                _ => position.z
            };
            
            rectTransform.localPosition = new Vector3(position.x, position.y, yPosition);
            rectTransform.rotation = Quaternion.Euler(0, 0, scaleAndRotation.z);
        }
        
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        
        public virtual void SetDefaultSize()
        {
            image.SetNativeSize();
        }
        
        public void SetAudio(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }
        
        public void SetTransparency(float alpha)
        {
            var newColor = image.color;
            newColor.a = alpha;

            image.color = newColor;
        }
        
        public void DisableSprite(Action onComplete = null)
        {
            Sequence?.Kill();
            Sequence = DOTween.Sequence();
            Sequence.Join(rectTransform.DOScale(
                Vector3.zero,
                GameSettings.bubbleDeleteDuration));
            Sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
            Sequence.Play();
        }
        
        public void RepositionGameObject(Vector3 moveVector)
        {
            gameObject.transform.position += moveVector;
        }
        
        public void SetAudioState(bool state)
        {
            if (state)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }

        public virtual void Reset()
        {
            audioSource.clip = null;
            image.sprite = null;
            image.enabled = false;
            image.color = Color.white;
        }
    }
}