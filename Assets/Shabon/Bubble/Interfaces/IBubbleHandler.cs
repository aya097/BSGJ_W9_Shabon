using System.Collections.Generic;
using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleHandler
    {
        // バブルを指定された方向と力で動かすメソッド
        void ApplyBreath(Vector3 direction, Vector3 position, float strength);

        // バブルを叩いて動かすメソッド
        void ApplyClap(float strength);
    }
}