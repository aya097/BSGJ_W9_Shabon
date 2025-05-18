using System;

namespace Shabon.Bubble
{
    public interface IBubbleBuildSetter
    {
        event Action OnReach;
        event Action OnClap;
        event Action OnDead;
        void SetBuildParam(IBubbleMover bubbleMover);
    }
}