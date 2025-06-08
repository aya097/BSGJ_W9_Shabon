using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        int BubbleScore { get; }
        Transform Transform { get; }
        public BubbleDeath Death { get; }
        void InvokeOnReach();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
    }
}

