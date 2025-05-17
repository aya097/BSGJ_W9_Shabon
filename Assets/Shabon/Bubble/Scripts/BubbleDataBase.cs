using UnityEngine;
using System.Collections.Generic;
using System;

namespace Shabon.Bubble
{
    [CreateAssetMenu(fileName = "BubbleDataBase", menuName = "Scriptable Objects/BubbleDataBase")]
    public class BubbleDataBase : ScriptableObject
    {
        [Serializable]
        public class BubbleData
        {
            public BubbleType bubbleType;
            public BubbleMono bubbleMono;
            public Vector3 initBubblePosition;
        }

        public List<BubbleData> bubbleData;
    }
}

