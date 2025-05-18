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
        public event Action? OnReach;
        public event Action? OnClap;
        public event Action? OnDead;

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
}

