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
        public event Action OnReach;
        public event Action OnClap;
        public event Action OnDead;

        // 仮の変数
        private Vector3 _bubbleMoveVelocity = new Vector3(0, 0, 1);
        private bool _isClap = false;
        private bool _isReached = false;
    

        void Update()
        {
            // 画面奥からbubbleが近づいてくる
            Move(_bubbleMoveVelocity);

            // calpされれば、
            if(_isClap)
            {
                OnClap?.Invoke();
                _isClap = false;
                OnDead?.Invoke();
            }

            // バブルがプレイヤー（手前？）に到達したら
            // 座標指定でも良き
            if(_isReached){
                OnReach?.Invoke();
                _isReached = false;
                Debug.Log("bubbleがプレイヤーに到達したよ");
            } 
        }

        /// <summary>
        /// バブルを指定した速度で移動させるメソッド
        /// </summary>
        public void Move(Vector3 velocity)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }
}

