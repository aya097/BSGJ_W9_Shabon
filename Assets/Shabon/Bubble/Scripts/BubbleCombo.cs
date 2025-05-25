#nullable enable

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルのコンボを管理するクラス
    /// </summary>
    public class BubbleCombo : IBubbleCombo
    {
        private List<IBubbleMono> _destroyedBubbles = new List<IBubbleMono>();
        private int _remainingChainBubbleCount;
        private int _comboCount = 0;

        /// <summary>
        /// destroyしたbubbleをコンボを加算するメソッド
        /// </summary>
        /// <param name="bubbleMono"></param>
        public void AddComboCount(IBubbleMono bubbleMono)
        {
            _destroyedBubbles.Add(bubbleMono);
            _comboCount++;

            // 1コンボ目のみ例外処理
            if (_remainingChainBubbleCount != 0) _remainingChainBubbleCount--;
            Debug.Log($"Combo :{_comboCount}");
        }

        /// <summary>
        /// 連鎖が残っているbubbleの数を加算するメソッド
        /// </summary>
        /// <param name="count"></param>
        public void AddRemainingChainBubbleCount(int count)
        {
            if (IsAssertMinusNum(count)) return;

            _remainingChainBubbleCount += count;
            if (_remainingChainBubbleCount == 0)
            {
                CalculateComboBonusScore();
                return;
            }
        }

        /// <summary>
        /// ボーナススコアの計算をするメソッド
        /// </summary>
        private void CalculateComboBonusScore()
        {
            Debug.Log("ボーナススコアの計算");

            _destroyedBubbles.Clear();
            _comboCount = 0;
            
            return;
        }

        
        static bool IsAssertMinusNum(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning($"{value} が負の数です");
                return true;
            }

            return false;
        }

    }

}

