#nullable enable
using System;
using Unity.VisualScripting;
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
            // target方向にy座標を変更せずに移動
            Vector3 directionToTarget = _targetTransform.position - _transform.position;
            directionToTarget.y = 0;
            _transform.Translate(directionToTarget.normalized * _forwardVelocity * Time.deltaTime, Space.World);
        }

        public void MoveByBreath(Vector3 direction)
        {
            _transform.Translate(direction * Time.deltaTime, Space.World); // 移動方向に移動
        }
    }
}