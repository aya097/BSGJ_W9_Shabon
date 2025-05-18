#nullable enable
using System;
using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルの動きを制御するクラス
    /// </summary>
    public class NormalBubbleMover : IBubbleMover
    {
        private readonly Transform _transform;  // 制御するBubbleのtransform

        public NormalBubbleMover(Transform transform)
        {
            _transform = transform;
        }

        public void MoveForward(float velocity)
        {
            _transform.Translate(_transform.forward * velocity * Time.deltaTime);   // 前方向に移動
        }

        public void MoveByBreath(Vector3 direction)
        {
            _transform.Translate(direction);    // 移動方向に移動
        }
    }
}