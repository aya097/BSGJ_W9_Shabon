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
        private readonly Transform _targetTransform; // PlayerのTransform
        private readonly float _forwardVelocity;

        public NormalBubbleMover(Transform transform, float forwardVelocity, Transform targetTransform)
        {
            _transform = transform;
            _forwardVelocity = forwardVelocity;
            _targetTransform = targetTransform;
        }

        public void MoveForward()
        {
            // Playerに向かって移動
            Vector3 directionToPlayer = (_targetTransform.position - _transform.position).normalized;
            _transform.Translate(directionToPlayer * _forwardVelocity * Time.deltaTime, Space.World);
        }

        public void MoveByBreath(Vector3 direction)
        {
            _transform.Translate(direction * Time.deltaTime, Space.World); // 移動方向に移動
        }
    }
}