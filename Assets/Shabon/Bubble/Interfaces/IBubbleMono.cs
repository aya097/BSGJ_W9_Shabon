using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        Transform Transform { get; }
        void InvokeOnReach();
        void InvokeOnDead();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
    }
}

