using Core.Infrastructure.Factories;
using Core.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class PoolContextInstaller : MonoInstaller
    {
        [Header("Pool parents")]
        [SerializeField] private Transform spritePoolParent;
        [SerializeField] private Transform bubblePoolParent;
        [SerializeField] private Transform sentencePoolParent;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject spritePrefab;
        [SerializeField] private GameObject sentencePrefab;
        
        public override void InstallBindings()
        {
            BindSpritePool();
            BindBubblePool();
            BindSentencePool();
            
            BindNodeFactory();
            BindMechanicsFactory();
            BindMonoFactory();
        }

        private void BindSpritePool()
        {
            Container
                .Bind<SpriteFactory>()
                .AsSingle()
                .WithArguments(spritePrefab);

            Container
                .Bind<SpritePool>()
                .AsSingle()
                .WithArguments(spritePoolParent);
        }
        private void BindBubblePool()
        {
            Container
                .Bind<BubbleFactory>()
                .AsSingle();

            Container
                .Bind<BubblePool>()
                .AsSingle()
                .WithArguments(bubblePoolParent);
        }
        private void BindSentencePool()
        {
            Container
                .Bind<SentenceFactory>()
                .AsSingle()
                .WithArguments(sentencePrefab);

            Container
                .Bind<SentencePool>()
                .AsSingle()
                .WithArguments(sentencePoolParent);
        }
        private void BindNodeFactory()
        {
            Container
                .Bind<NodeFactory>()
                .AsSingle();
        }
        private void BindMechanicsFactory()
        {
            Container
                .Bind<MechanicsFactory>()
                .AsSingle();
        }
        private void BindMonoFactory()
        {
            Container
                .Bind<MonoFactory>()
                .AsSingle();
        }
    }
}