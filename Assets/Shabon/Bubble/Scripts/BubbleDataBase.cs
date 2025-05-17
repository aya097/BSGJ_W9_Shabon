using UnityEngine;
using Shabon.Bubble;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "BubbleDataBase", menuName = "Scriptable Objects/BubbleDataBase")]
public class BubbleDataBase : ScriptableObject
{
    [Serializable]
    public class BubbleData
    {
        public BubbleType bubbleType;
        public GameObject bubblePrefab;
    }

    public List<BubbleData> bubbleData;
}
