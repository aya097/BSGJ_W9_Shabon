using System;
using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleBuildSetter
    {
        event Action OnReach;
        event Action<OnClapArg> OnClap;
        event Action<OnBreathArg> OnBreath;
        void SetBuildParam(IBubbleMover bubbleMover, IAreaChecker areaChecker, IBubbleData bubbleData, BubbleCluster bubbleCluster);
    }
}