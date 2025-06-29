using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        bool IsReached { get; set; }
        bool IsAttacking { get; set; }
        bool IsStop { get; }
        bool IsClapable { get; set; }
        int BubbleScore { get; }
        Transform Transform { get; }
        public BubbleDeath Death { get; }
        void InvokeOnReach();
        void InvokeOnClap(OnClapArg arg);
        void InvokeOnBreath(OnBreathArg arg);
        void Stop();
        void Resume();

        int BossHitPoint { get => 0; }
        void Back() { }
        void DecreaseHp(int bossHitPoint) { }

    }
}

