using UnityEngine;
using System;


namespace Shabon.Bubble
{
    public interface IBubbleMono
    {
        event Action OnReach;
        event Action OnClap;
        event Action OnDead;

        // 引数は、毎秒の移動量（速度）
        void Move(Vector3 velocity);
    }
}

