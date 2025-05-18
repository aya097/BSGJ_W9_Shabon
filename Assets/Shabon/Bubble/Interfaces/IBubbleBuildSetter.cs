using System;
using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleBuildSetter
    {
        Transform Transform { get; }
        event Action OnReach;
        event Action OnDead;
        event Action<OnClapArg> OnClap;
        event Action<OnBreathArg> OnBreath;
        void SetBuildParam(IBubbleMover bubbleMover);
    }
}