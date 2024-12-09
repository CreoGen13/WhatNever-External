using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Node.Panel
{
    [CreateAssetMenu(fileName = "Panel", menuName = "Containers/Panel", order = 0)]
    [Serializable]
    public class PanelContainer : ScriptableObject
    {
        public List<NodeLinkData> nodeLinks = new();
        public List<StackNodeData> stackNodeData = new();
    }
}
