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
        public bool IsReached => _isReached;
        public bool IsAttacking
        {
            get { return _isAttacking; }
            set { _isAttacking = value; }
        }
        public int BubbleScore => _bubbleScore;
        public Transform? Transform => transform;
        public BubbleDeath Death => _bubbleDeath;
        public event Action? OnReach;
        public event Action<OnClapArg>? OnClap;
        public event Action<OnBreathArg>? OnBreath;

        private IBubbleMover _bubbleMover = null!;  // バブルを動かすクラス
        private BubbleDeath _bubbleDeath = null!;   // バブルの割れる処理
        private IAreaChecker _waitAreaChecker = null!;
        private int _bubbleScore;
        private bool _isReached = false;
        private bool _isAttacking = false;
        private bool _isStop = false;

        void Update()
        {


            // 到達したら移動しない
            if (_isReached) return;

            // 停止中だったら動かない
            if (_isStop) return;

            _bubbleMover.MoveForward();

            // waitArea
            if (_waitAreaChecker.IsInArea(transform.position))
            {
                _isReached = true;
                InvokeOnReach();
            }
        }

        /// <summary>
        /// ビルドの際にパラメータを注ぐ用のクラス
        /// </summary>
        public void SetBuildParam(
            IBubbleMover bubbleMover,
            BubbleDeath bubbleDeath,
            IAreaChecker areaChecker,
            IBubbleData bubbleData)
        {
            _bubbleMover = bubbleMover;
            _bubbleDeath = bubbleDeath;
            _waitAreaChecker = areaChecker;
            _bubbleScore = bubbleData.BubbleScore;
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

