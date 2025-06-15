using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        bool IsReached { get; }
        bool IsAttacking { get; set; }
        int BubbleScore { get; }
        Transform Transform { get; }
        public BubbleDeath Death { get; }
        void InvokeOnReach();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
        void Stop();
        void Resume();
    }
}

