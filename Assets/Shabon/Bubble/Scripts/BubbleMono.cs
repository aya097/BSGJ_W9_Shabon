#nullable enable

using System;
using UnityEngine;
using VContainer;
using Shabon.Score;

namespace Shabon.Bubble
{
    /// <summary>
    /// 通常バブル（ボスバブルではない）の動きやイベントを管理するためのクラス
    /// </summary>
    public class BubbleMono : MonoBehaviour, IBubbleMono, IBubbleBuildSetter
    {
        // 息が吹かれているか
        public bool IsBreathing
        {
            get { return _isBreathing; }
            set { _isBreathing = value; }
        }
        // 攻撃位置に到達している   
        public bool IsReached
        {
            get { return _isReached; }
            set { _isReached = value; }
        }
        // 攻撃中か
        public bool IsAttacking
        {
            get { return _isAttacking; }
            set { _isAttacking = value; }
        }
        // 停止させられているか
        public bool IsStop => _isStop;
        // Clapできるか？
        public bool IsClapable
        {
            get { return _isClapable; }
            set { _isClapable = value; }
        }
        public int BubbleScore => _bubbleScore;
        public Transform? Transform => transform;
        public event Action? OnReach;
        public event Action<OnClapArg>? OnClap;
        public event Action<OnBreathArg>? OnBreath;

        protected IBubbleMover _bubbleMover = null!;  // バブルを動かすクラス
        protected IAreaChecker _waitAreaChecker = null!;
        protected BubbleCluster _bubbleCluster = null!;
        protected int _bubbleScore;
        protected bool _isBreathing = false;
        protected bool _isReached = false;
        private bool _isAttacking = false;
        protected bool _isStop = false;
        private bool _isClapable = false;

        //Bubble同士の距離
        private float _bubbleRadius = 0.2f;
        // ズレる速さ
        private float _separateForce = 5.0f;

        protected virtual void Update()
        {
            // 到達したら移動しない
            if (_isReached) return;

            // 停止中だったら動かない
            if (_isStop) return;

            // 常に重なり解消を先に実行
            SeparateIfOverlapping();

            // 息が吹かれてるときは前進しないように
            if (_isBreathing == false) _bubbleMover.MoveForward();

            // waitArea
            if (_waitAreaChecker.IsInArea(transform.position))
            {
                _isReached = true;
                InvokeOnReach();
            }
        }

        private void SeparateIfOverlapping()
        {
            if (_bubbleCluster == null) return;

            foreach (var other in _bubbleCluster.Bubbles)
            {
                if (ReferenceEquals(other, this)) continue;
                Vector3 dir = transform.position - other.Transform.position;
                float dist = dir.magnitude;
                if (dist < _bubbleRadius * 2f && dist > 0.01f)
                {
                    // 常に左右（x方向）のみ押し出す
                    Vector3 pushDir = dir.x > 0 ? Vector3.right : Vector3.left;
                    _bubbleMover.AddForce(pushDir * _separateForce * Time.deltaTime);
                }

                _bubbleMover.UpdateSeparate();
            }
        }

        /// <summary>
        /// ビルドの際にパラメータを注ぐ用のクラス
        /// </summary>
        public virtual void SetBuildParam(
            IBubbleMover bubbleMover,
            IAreaChecker areaChecker,
            IBubbleData bubbleData,
            BubbleCluster bubbleCluster
        )
        {
            _bubbleMover = bubbleMover;
            _waitAreaChecker = areaChecker;
            _bubbleScore = bubbleData.BubbleScore;
            _bubbleCluster = bubbleCluster;
        }

        /// <summary>
        /// 前方に到達したとき
        /// </summary>
        public void InvokeOnReach()
        {
            OnReach?.Invoke();
        }

        /// <summary>
        /// Clapされたとき
        /// </summary>
        public void InvokeOnClap(OnClapArg arg)
        {
            OnClap?.Invoke(arg);
        }
        /// <summary>
        /// Breathされたとき
        /// </summary>
        public void InvokeOnBreath(OnBreathArg arg)
        {
            OnBreath?.Invoke(arg);
        }

        public void Stop()
        {
            _isStop = true;
        }
        public void Resume()
        {
            _isStop = false;
        }
    }


    /// <summary>
    /// OnClapの引数
    /// </summary>
    public class OnClapArg
    {
        public float Strength { get; }  // Clapの強さ

        public OnClapArg(float strength)
        {
            Strength = strength;
        }
    }

    /// <summary>
    /// OnBreathの引数
    /// </summary>
    public class OnBreathArg
    {
        public float Strength { get; }  // 息の強さ
        public Vector3 Direction { get; }   // 息の向き
        public Vector3 Position { get; }    // 息の原点

        public OnBreathArg(float strength, Vector3 direction, Vector3 position)
        {
            Strength = strength;
            Direction = direction;
            Position = position;
        }
    }


}

