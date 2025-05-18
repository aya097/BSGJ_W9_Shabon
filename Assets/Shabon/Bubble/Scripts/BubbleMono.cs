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



        // 仮の変数(DataBaseから取得するのがいいかも、ビルドする)
        private Vector3 _bubbleMoveVelocity = new Vector3(0, 0, -1);

        void Update()
        {

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

