using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        event Action OnReach;
        event Action OnDead;
        event Action<OnClapArg> OnClap;
        event Action<OnBreathArg> OnBreath;
    }
}

