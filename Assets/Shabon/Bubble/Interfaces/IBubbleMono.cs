using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        int BubbleScore{ get; }
        Transform Transform { get; }
        void InvokeOnReach();
        void InvokeOnDead();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
    }
}

