using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Node.Panel

{
    [Serializable]
    public class StackNodeData
    {
        public string name;
        public string label;
        public Vector2 position;
        public List<NodeData> nodeData = new();
    }
}