using System;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace Core.Base.Classes
{
    public abstract class BasePresenter<TModel, TView>
        where TModel : BaseModel<TModel>
        where TView : BaseView
    {
        protected readonly TModel Model;
        protected readonly TView View;

        private readonly CompositeDisposable _disposable = new();

        protected BasePresenter(TModel model, TView view)
        {
            View = view;
            Model = model;

            View.OnDispose += Dispose;
        }
        
        protected abstract void InitSubscriptions();
        protected virtual void InitActions(){}

        protected void AddSubscription<T>(Func<TModel, T> selector, Action<T> onValueChanged)
        {
            Model
                .Observe()
                .Select(selector)
                .Subscribe(value =>
                {
                    onValueChanged?.Invoke(value);
                })
                .AddTo(_disposable);
        }
        protected void AddSubscriptionWithDistinct<T>(Func<TModel, T> selector, Action<T> onValueChanged)
        {
            Model
                .Observe()
                .Select(selector)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(value => { onValueChanged?.Invoke(value); })
                .AddTo(_disposable);
        }
        protected void AddCollectionSubscription<T>(Func<TModel, ReactiveCollection<T>> selector, Action<T, T> onValueChanged)
        {
            selector?.Invoke(Model)
                .ObserveReplace()
                .Subscribe((value) =>
                {
                    onValueChanged?.Invoke(value.OldValue, value.NewValue);
                })
                .AddTo(_disposable);
        }
        protected void AddCollectionSubscriptionWithDistinct<T>(Func<TModel, ReactiveCollection<T>> selector, Action<T, T> onValueChanged)
        {
            selector?.Invoke(Model)
                .ObserveReplace()
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe((value) =>
                {
                    onValueChanged?.Invoke(value.OldValue, value.NewValue);
                })
                .AddTo(_disposable);
        }
        protected void AddDictionarySubscription<TKey, TValue>(Func<TModel, ReactiveDictionary<TKey, TValue>> selector, Action<TKey, TValue, TValue> onValueChanged)
        {
            selector?.Invoke(Model)
                .ObserveReplace()
                .Subscribe((value) =>
                {
                    onValueChanged?.Invoke(value.Key, value.OldValue, value.NewValue);
                })
                .AddTo(_disposable);
        }
        protected void AddDictionarySubscriptionWithDistinct<TKey, TValue>(Func<TModel, ReactiveDictionary<TKey, TValue>> selector, Action<TKey, TValue, TValue> onValueChanged)
        {
            selector?.Invoke(Model)
                .ObserveReplace()
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe((value) =>
                {
                    onValueChanged?.Invoke(value.Key, value.OldValue, value.NewValue);
                })
                .AddTo(_disposable);
        }

        private void Dispose()
        {
            _disposable.Dispose();
        }
    }
}