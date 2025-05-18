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

            _transform.Translate(new Vector3(0, 0, -1f) * _forwardVelocity * Time.deltaTime);   // -Z方向に移動
        }

        public void MoveByBreath(Vector3 direction)
        {
            _transform.Translate(direction * Time.deltaTime);    // 移動方向に移動
        }
    }
}