using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        bool IsBreathing { get; set; }
        bool IsReached { get; set; }
        bool IsAttacking { get; set; }
        bool IsStop { get; }
        bool IsSeparatable { get; set; }
        bool IsClapable { get; set; }
        int BubbleScore { get; }
        Transform Transform { get; }
        void InvokeOnReach();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
        void Stop();
        void Resume();

        // BossBubble
        int BossHitPoint { get => 0; }
        void Back() { }
        void DecreaseHp(int bossHitPoint) { }

    }
}

