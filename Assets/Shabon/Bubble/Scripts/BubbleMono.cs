#nullable enable

using System;
using UnityEngine;

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


        void Update()
        {
            _bubbleMover.MoveForward();
        }


        /// <summary>
        /// ビルドの際にパラメータを注ぐ用のクラス
        /// </summary>
        public void SetBuildParam(IBubbleMover bubbleMover)
        {
            _bubbleMover = bubbleMover;
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

