using System;
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Clap
{
    public class ClapModel
    {
        private readonly IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー

        public ClapModel(IBubbleHandler bubbleHandler)
        {
            // コンストラクタでバブルハンドラーを受け取る
            _bubbleHandler = bubbleHandler;
        }
    }
}