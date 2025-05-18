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
        private readonly float _forwardVelocity;

        public NormalBubbleMover(Transform transform, float forwardVelocity)
        {
            _transform = transform;
            _forwardVelocity = forwardVelocity;
        }

        public void MoveForward()
        {
            _transform.Translate(_transform.forward * _forwardVelocity * Time.deltaTime);   // 前方向に移動
        }

        public void MoveByBreath(Vector3 direction)
        {
            _transform.Translate(direction);    // 移動方向に移動
        }
    }
}