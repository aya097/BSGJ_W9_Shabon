#nullable enable

using System;
using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// 通常バブル（ボスバブルではない）の動きやイベントを管理するためのクラス
    /// </summary>
    public class BubbleMono : MonoBehaviour, IBubbleMono
    {
        public event Action? OnReach;
        public event Action? OnClap;
        public event Action? OnDead;

        // 仮の変数
        private Vector3 _bubbleMoveVelocity = new Vector3(0, 0, 1);


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
        /// バブルがプレイヤーに到達したときに呼び出されるメソッド
        /// </summary>
        public void Reach()
        {
            OnReach?.Invoke();
        }

        /// <summary>
        /// バブルがクラップされたときに呼び出されるメソッド
        /// </summary>
        public void Clap()
        {
            OnClap?.Invoke();
        }

        /// <summary>
        /// バブルが消えたときに呼び出されるメソッド
        /// </summary>
        public void Dead()
        {
            OnDead?.Invoke();
        }
    }
}

