using UnityEngine;
using Shabon.Bubble;
using VContainer;

namespace Shabon.Breath
{

    public class BreathModel
    {
        public Vector3 Direction => _currentDirection;
        public Vector3 Position => _originPosition;

        private readonly IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー
        private Vector3 _originPosition;    // 息の発生位置
        private float _maxDegree = 60f; // 正面を0度として
        private Vector3 _currentDirection;  // 現在の向き

        [Inject]
        public BreathModel(IBubbleHandler bubbleHandler)
        {
            // コンストラクタでバブルハンドラーを受け取る
            _bubbleHandler = bubbleHandler;

            _currentDirection = new Vector3(0, 0, 1f);
        }

        /// <summary>
        /// 息を吹く
        /// </summary>
        /// <param name="strength"></param>
        public void ApplyBreath(float strength)
        {
            _bubbleHandler.ApplyBreath(_currentDirection, _originPosition, strength);
        }

        /// <summary>
        /// -1~1を受け取りそれに対応した角度で向きを設定
        /// </summary>
        /// <param name="ratio"></param>
        public void SetDirection(float ratio)
        {
            if (ratio > 1)
            {
                Debug.LogWarning($"ratio {ratio} is greater than 1");
                ratio = 1;
            }
            else if (ratio < -1)
            {
                Debug.LogWarning($"ratio {ratio} is less than 0");
                ratio = -1;
            }

            // 現在の向きを計算
            float degree = _maxDegree * ratio;
            float rad = degree / 180f * Mathf.PI;
            // z軸向きに
            _currentDirection = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
        }

        /// <summary>
        /// 息の発生源を設定
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosition(Vector3 pos)
        {
            _originPosition = pos;
        }
    }
}