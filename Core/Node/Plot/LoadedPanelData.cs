using Core.Base.Classes;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using Core.Node.Panel;
using UnityEngine;

namespace Core.Node.Plot
{
    public class LoadedPanelData : BaseLoadedData
    {
        public string Name;
        public Vector2 Position;
        
        public PanelContainer PanelContainer;
        public PanelType PanelType;
        public bool DeletePreviousPanel;
        public bool HasCondition;
        public GameVariable PlotCondition;
        public bool HasPrefab;
        public GameObject Prefab;
        public bool HasAudio;
        public AudioClip Audio;
    }
}