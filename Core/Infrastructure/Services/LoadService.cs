using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base.Classes;
using Core.Infrastructure.Utils;
using Core.Node.Panel;
using Core.Node.Plot;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Infrastructure.Services
{
    public class LoadService : BaseService
    {
        private class AssetReferenceHandleUsages
        {
            public AssetReference AssetReference;
            public AsyncOperationHandle Handle;
            public int Usages;
        }
        private class HandleWithGuid
        {
            public AsyncOperationHandle Handle;
            public string Guid;
        }
            
        private AsyncOperationHandle<PlotContainer> _plotHandle;
        private Dictionary<PanelData, List<HandleWithGuid>> _panelHandles;
        private Dictionary<NodeData, List<HandleWithGuid>> _nodeHandles;
        private Dictionary<string, AssetReferenceHandleUsages> _assetReferenceUsages;

        public override void Initialize()
        {
            _panelHandles = new Dictionary<PanelData, List<HandleWithGuid>>();
            _nodeHandles = new Dictionary<NodeData, List<HandleWithGuid>>();
            _assetReferenceUsages = new Dictionary<string, AssetReferenceHandleUsages>();
        }

        #region Plot

        public async UniTask<PlotContainer> LoadPlot()
        {
            _plotHandle = Addressables.LoadAssetAsync<PlotContainer>("Assets/Containers/Plot.asset");
            var result = await _plotHandle;
            
            return result;
        }
        
        public void UnloadPlot()
        {
            if (_plotHandle.IsValid())
            {
                Addressables.Release(_plotHandle);
            }
            else
            {
                this.LogError("Plot is not presented in handles");
            }
        }

        #endregion
        #region Panel

        public async UniTask<LoadedPanelData> LoadPanel(PanelData panelData)
        {
            List<HandleWithGuid> handles = new List<HandleWithGuid>();
            AudioClip audio = null;
            GameObject prefab = null;
            PanelContainer panelContainer;
            HandleWithGuid handleWithGuid = null;

            bool exists = _panelHandles.TryGetValue(panelData, out var handlesWithGuid);
            
            if(exists)
            {
                handleWithGuid = handlesWithGuid.FirstOrDefault(handle => handle.Guid == panelData.panelContainer.AssetGUID);
            }
            
            if (exists && handleWithGuid != null)
            {
                var handle = handleWithGuid.Handle;

                await handle;

                panelContainer = (PanelContainer)handle.Result;
            }
            else
            {
                var containerHandle = Addressables.LoadAssetAsync<PanelContainer>(panelData.panelContainer);
                panelContainer = await containerHandle;
                handles.Add(new HandleWithGuid
                {
                    Handle = containerHandle,
                    Guid = panelData.panelContainer.AssetGUID
                });
            }
            
            if (panelData.hasAudio)
            {
                if (exists)
                {
                    handleWithGuid = handlesWithGuid.FirstOrDefault(handle => handle.Guid == panelData.audio.AssetGUID);
                }
                
                if (exists && handleWithGuid != null)
                {
                    var handle = handleWithGuid.Handle;

                    await handle;

                    audio = (AudioClip)handle.Result;
                }
                else
                {
                    var handle = Addressables.LoadAssetAsync<AudioClip>(panelData.audio);
                    audio = await handle;
                
                    handles.Add(new HandleWithGuid
                    {
                        Handle = handle,
                        Guid = panelData.audio.AssetGUID
                    });
                }
            }
            
            if (panelData.hasPrefab)
            {
                if (exists)
                {
                    handleWithGuid = _panelHandles[panelData].FirstOrDefault(handle => handle.Guid == panelData.prefab.AssetGUID);
                }
                
                if (exists && handleWithGuid != null)
                {
                    var handle = handleWithGuid.Handle;

                    await handle;

                    prefab = (GameObject)handle.Result;
                }
                else
                {
                    var handle = Addressables.LoadAssetAsync<GameObject>(panelData.prefab);
                    prefab = await handle;

                    handles.Add(new HandleWithGuid
                    {
                        Handle = handle,
                        Guid = panelData.prefab.AssetGUID
                    });
                }
            }

            LoadedPanelData loadedPanelData = new LoadedPanelData
            {
                Name = panelData.name,
                Position = panelData.position,
                PanelContainer = panelContainer,
                PanelType = panelData.panelType,
                DeletePreviousPanel = panelData.deletePreviousPanel,
                HasCondition = panelData.hasCondition,
                PlotCondition = panelData.plotCondition,
                HasPrefab = panelData.hasPrefab,
                Prefab = prefab,
                HasAudio = panelData.hasAudio,
                Audio = audio
            };

            _panelHandles.TryAdd(panelData, handles);
            
            return loadedPanelData;
        }


        private bool UnloadPanelInternal(PanelData panelData)
        {
            if (_panelHandles.ContainsKey(panelData))
            {
                panelData.panelContainer.ReleaseAsset();

                if (panelData.hasAudio)
                {
                    panelData.audio.ReleaseAsset();
                }
                
                if (panelData.hasPrefab)
                {
                    panelData.prefab.ReleaseAsset();
                }

                return true;
            }
            else
            {
                this.LogError("PanelData " + panelData + " is not presented in handles");

                return false;
            }
        }
        public void UnloadPanel(PanelData panelData)
        {
            if (UnloadPanelInternal(panelData))
            {
                _panelHandles.Remove(panelData);
            }
        }
        public void UnloadAllPanelData()
        {
            UnloadAllNodeData();
            
            foreach (var handles in _panelHandles)
            {
                UnloadPanelInternal(handles.Key);
            }
            
            _panelHandles.Clear();
        }

        #endregion
        #region Node

        public async UniTask<LoadedNodeData> LoadNodeData(NodeData nodeData)
        {
            List<HandleWithGuid> handles = new List<HandleWithGuid>();
            AudioClip audioClip = null;
            GameObject gameObject = null;
            Sprite sprite = null;
            RuntimeAnimatorController animatorController = null;
            string text = null;
            
            Type type;

            try
            {
                type = typeof(BaseNodeProcessor).Assembly.GetType(nodeData.nodeType);
            }
            catch (Exception)
            {
                this.LogError("Assembly has no type as " + nodeData.nodeType);
                throw;
            }
            
            if (nodeData.hasAudio)
            {
                if (nodeData.audioClip.AssetGUID == "")
                {
                    this.LogError("Node " + nodeData.name + " marked as has audio but audioClip is null");
                }
                else
                {
                    var handle = TryGetHandle<AudioClip>(nodeData.audioClip, ref handles);
                    await handle;
                    audioClip = (AudioClip)handle.Result;
                }
            }
            
            if (nodeData.hasAnimation)
            {
                if (nodeData.animatorController.AssetGUID == "")
                {
                    this.LogError("Node " + nodeData.name + " marked as has animation but animatorController is null");
                }
                else
                {
                    var handle = TryGetHandle<RuntimeAnimatorController>(nodeData.animatorController, ref handles);
                    await handle;
                    animatorController = (RuntimeAnimatorController)handle.Result;
                }
            }
            
            if (nodeData.gameObject.AssetGUID != "")
            {
                var handle = TryGetHandle<GameObject>(nodeData.gameObject, ref handles);
                await handle;
                gameObject = (GameObject)handle.Result;
            }
            
            if (nodeData.sprite.AssetGUID != "")
            {
                var handle = TryGetHandle<Sprite>(nodeData.sprite, ref handles);
                await handle;
                sprite = (Sprite)handle.Result;
            }

            if (!nodeData.localizedText.IsEmpty)
            {
                var handle = nodeData.localizedText.GetLocalizedStringAsync();
                text = await handle;
                
                // TODO: control localization loading
                // handles.Add(handle);
            }
            
            LoadedNodeData loadedNodeData = new LoadedNodeData(nodeData)
            {
                Type = type,
                AudioClip = audioClip,
                GameObject = gameObject,
                Sprite = sprite,
                AnimatorController = animatorController,
                Text = text,
            };
            
            _nodeHandles.TryAdd(nodeData, handles);
            
            return loadedNodeData;
        }

        private bool UnloadNodeDataInternal(NodeData nodeData)
        {
            if (_nodeHandles.ContainsKey(nodeData))
            {
                if (nodeData.hasAnimation && nodeData.animatorController.AssetGUID != "")
                {
                    TryRemoveHandle(nodeData.animatorController);
                }
                
                if (nodeData.hasAudio && nodeData.audioClip.AssetGUID != "")
                {
                    TryRemoveHandle(nodeData.audioClip);
                }

                if (nodeData.gameObject.AssetGUID != "")
                {
                    TryRemoveHandle(nodeData.gameObject);
                }

                if (nodeData.sprite.AssetGUID != "")
                {
                    TryRemoveHandle(nodeData.sprite);
                }

                return true;
            }

            this.LogError("NodeData " + nodeData + " is not presented in handles");

            return false;
        }
        public void UnloadNodeData(NodeData nodeData)
        {
            if(UnloadNodeDataInternal(nodeData))
            {
                _nodeHandles.Remove(nodeData);
            }
        }
        public void UnloadAllNodeData()
        {
            foreach (var handles in _nodeHandles)
            {
                UnloadNodeDataInternal(handles.Key);
            }
            
            _nodeHandles.Clear();
        }

        #endregion
        
        public void UnloadAll()
        {
            UnloadAllPanelData();
            UnloadPlot();
        }

        private AsyncOperationHandle TryGetHandle<T>(AssetReference assetReference, ref List<HandleWithGuid> handles)
        {
            var guid = assetReference.AssetGUID;
            
            if (_assetReferenceUsages.TryGetValue(guid, out var handleUsage))
            {
                _assetReferenceUsages[guid].Usages++;
                
                handles.Add(new HandleWithGuid
                {
                    Handle =  handleUsage.Handle,
                    Guid = guid,
                });
                
                return handleUsage.Handle;
            }

            var newHandle = Addressables.LoadAssetAsync<T>(assetReference);
            
            _assetReferenceUsages.Add(guid, new AssetReferenceHandleUsages()
            {
                AssetReference = assetReference,
                Handle = newHandle,
                Usages = 1,
            });
            
            handles.Add(new HandleWithGuid
            {
                Handle = newHandle,
                Guid = guid,
            });

            return newHandle;
        }
        private void TryRemoveHandle(AssetReference assetReference)
        {
            var guid = assetReference.AssetGUID;

            if (!_assetReferenceUsages.TryGetValue(guid, out var handleUsage))
            {
                return;
            }
            
            handleUsage.Usages--;
                
            if (handleUsage.Usages == 0)
            {
                handleUsage.AssetReference.ReleaseAsset();
                    
                _assetReferenceUsages.Remove(guid);
            }
        }
    }
}