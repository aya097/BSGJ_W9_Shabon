using System;
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Clap
{

    public class Clap

    // todo monoなくす
    public class Clap : MonoBehaviour

    {
        public static event Action OnClap; // Clapイベントを通知するためのデリゲート


        private IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー

        // todo IBubbleHandlerのClapを呼ぶ


        public Clap(IBubbleHandler bubbleHandler)
        {
            // コンストラクタでバブルハンドラーを受け取る
            _bubbleHandler = bubbleHandler;
        }

        public void CheckForClap(float xPosition, bool isClapKeyPressed)
        {
            // 指定された位置がClap範囲内で、Clapキーが押されている場合にClapアクションを実行
            if (xPosition >= -20f && xPosition <= 20f)
            {
                if (isClapKeyPressed)
                {
                    ClapAction();
                }
            }
        }

        private void ClapAction()
        {
            // Clapイベントを発火し、バブルハンドラーのClapメソッドを呼び出す
            OnClap?.Invoke();
            _bubbleHandler.Clap();

            // バブルがClapされた場合、OnClapイベントを発火
            _bubbleHandler.TriggerOnClap(); // BubbleHandler経由でOnClapイベントを発火
        }

        public void Update()
        {
            _bubbleHandler.HandleInput();
        }
    }
}