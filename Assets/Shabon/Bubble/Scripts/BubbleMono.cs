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
        private IDirtValue _dirtValue = null!; // DirtValueを保持

        [Inject]
        public void Initialize(IDirtValue dirtValue)
        {
            _dirtValue = dirtValue;
        }

        void Update()
        {
            _bubbleMover.MoveForward();

            // z座標が-3.0以下になった場合にDirtValueを増加
            if (transform.position.z <= -3.0f)
            {
                _dirtValue.Increase(1);
                InvokeOnReach(); // イベントを発火
                Destroy(gameObject); // バブルを削除
            }
        }

        /// <summary>
        /// ビルドの際にパラメータを注ぐ用のクラス
        /// </summary>
        public void SetBuildParam(IBubbleMover bubbleMover)
        {
            _bubbleMover = bubbleMover;
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

