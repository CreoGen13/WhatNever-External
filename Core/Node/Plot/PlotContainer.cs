using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Node.Plot
{
    [CreateAssetMenu(fileName = "Plot", menuName = "Containers/Plot", order = 1)]
    [Serializable]
    public class PlotContainer : ScriptableObject
    {
        public List<NodeLinkData> nodeLinks = new();
        public List<PanelData> panelNodeData = new();
    }
}