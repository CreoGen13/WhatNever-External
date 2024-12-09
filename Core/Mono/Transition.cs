using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core.Mono
{
    public class Transition : MonoBehaviour
    {
        private const string EntryName = "Entry";
        private const string StartName = "Start";
        private const string EndName = "End";
        
        [SerializeField] private Animator animator;

        private Sequence _sequence;

        [Inject]
        private void Construct()
        {
        }

        public void Play(Action onComplete)
        {
            Reset();
            animator.Play(StartName);
            animator.Update(0);
            animator.speed = 1;

            var duration = animator.GetCurrentAnimatorStateInfo(0).length;
                
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(duration);
            _sequence.AppendCallback(() => { onComplete?.Invoke(); });
        }
        public void Stop(Action onComplete)
        {
            Reset();
            animator.Play(EndName);
            animator.Update(0);
            animator.speed = 1;

            var duration = animator.GetCurrentAnimatorStateInfo(0).length;
                
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(duration);
            _sequence.AppendCallback(() =>
            {
                onComplete?.Invoke();

                Destroy(gameObject);
            });
        }

        private void Reset()
        {
            animator.Play(EntryName);
            animator.Update(0);
        }
    }
}