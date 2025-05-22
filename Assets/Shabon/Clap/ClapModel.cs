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
            _bubbleHandler = bubbleHandler;
        }

        public void PerformClap(Vector3 position, float strength)
        {
            _bubbleHandler.ApplyClap(position, strength); // Clap処理を実行
        }
    }
}