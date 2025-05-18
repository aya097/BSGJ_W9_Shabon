using System;

namespace Shabon.Bubble
{
    public interface IBubbleBuildSetter
    {
        event Action OnReach;
        event Action<OnClapArg> OnClap;
        event Action<OnBreathArg> OnDead;
        void SetBuildParam(IBubbleMover bubbleMover);
    }
}