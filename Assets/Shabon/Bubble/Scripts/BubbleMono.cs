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

        // 仮の変数(DataBaseから取得するのがいいかも、ビルドする)
        private Vector3 _bubbleMoveVelocity = new Vector3(0, 0, -1);

        void Update()
        {
            // 画面奥からbubbleが近づいてくる
            Move(_bubbleMoveVelocity);
        }

        /// <summary>
        /// バブルを指定した速度で移動させるメソッド
        /// </summary>
        public void Move(Vector3 velocity)
        {
            transform.position += velocity * Time.deltaTime;
        }

        /// <summary>
        /// ビルドの際にパラメータを注ぐ用のクラス
        /// </summary>
        public void SetBuildParam()
        {

        }
    }
}

