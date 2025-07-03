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

        private Vector3 _separateSpeed = Vector3.zero; // 横移動の速さ
        private float _decreaseSpeed = 1f; // 横移動の減速速度

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

        public void UpdateSeparate()
        {
            _transform.Translate(_separateSpeed * Time.deltaTime, Space.World);

            Vector3 decreasedSpeed = _separateSpeed - _separateSpeed.normalized * _decreaseSpeed * Time.deltaTime;    // 減衰速度だけ減らす。
                                                                                                                      // 向きが変わっていたら、
            if (Vector3.Dot(decreasedSpeed, _separateSpeed) < 0)
            {
                _separateSpeed = Vector3.zero; // 向きが変わったらリセット
            }
            else
            {
                _separateSpeed = decreasedSpeed; // 減衰速度を適用
            }
        }

        public void AddForce(Vector3 force)
        {
            _separateSpeed += force;
        }
    }
}