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
        public Transform Transform => transform;
        public event Action? OnReach;
        public event Action? OnDead;
        public event Action<OnClapArg>? OnClap;
        public event Action<OnBreathArg>? OnBreath;

        private IBubbleMover _bubbleMover = null!;  // バブルを動かすクラス
        private IAreaChecker _waitAreaChecker = null!;

        private bool _isReached = false;

        void Update()
        {
            // 到達したら移動しない
            if (_isReached) return;

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
            IAreaChecker areaChecker)
        {
            _bubbleMover = bubbleMover;
            _waitAreaChecker = areaChecker;
        }
        /// <summary>
        /// 前方に到達したとき
        /// </summary>
        public void InvokeOnReach()
        {
            OnReach?.Invoke();
        }
        /// <summary>
        /// 割れたとき
        /// </summary>
        public void InvokeOnDead()
        {
            OnDead?.Invoke();
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

