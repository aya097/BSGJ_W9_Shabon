using System;
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Clap
{
    public class Clap
    {
        private readonly IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー

        public Clap(IBubbleHandler bubbleHandler)
        {
            // コンストラクタでバブルハンドラーを受け取る
            _bubbleHandler = bubbleHandler;
        }
    }
}