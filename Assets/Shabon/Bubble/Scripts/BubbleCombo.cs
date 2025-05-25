#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルのコンボを管理するクラス
    /// </summary>
    public class BubbleCombo : IBubbleCombo
    {
        private readonly List<IBubbleMono> _destroyedBubbles = new ();
        private readonly List<IBubbleMono> _remainingChainBubbles = new ();
        private int _comboCount = 0;

        /// <summary>
        /// destroyしたbubbleをコンボを加算するメソッド
        /// </summary>
        /// <param name="bubbleMono"></param>
        public void AddComboCount(IBubbleMono bubbleMono)
        {
            _destroyedBubbles.Add(bubbleMono);
            _comboCount++;
            Debug.Log($"Combo :{_comboCount}");

            RemoveChainedBubble(bubbleMono);
        }

        /// <summary>
        /// 連鎖済みのbubbleをListから除去
        /// </summary>
        /// <param name="bubbleMono"></param>
        public void RemoveChainedBubble(IBubbleMono bubbleMono)
        {
            // 1コンボ目のみ例外処理
            if (_remainingChainBubbles.Count() == 0 ) return;
            _remainingChainBubbles.Remove(bubbleMono);
        }

        /// <summary>
        /// 連鎖が残っているbubbleの数を加算するメソッド
        /// </summary>
        /// <param name="count"></param>
        public void AddRemainingChainBubble(IEnumerable<IBubbleMono> bubbleMonos)
        {
            foreach (IBubbleMono bubbleMono in bubbleMonos)
            {
                if (_remainingChainBubbles.Contains(bubbleMono)) return;
                _remainingChainBubbles.Add(bubbleMono);
            }

            //Debug.Log($"remainingChainBubbleCount : {_remainingChainBubbles.Count()}");

            if (_remainingChainBubbles.Count() != 0) return;
            
            // 連鎖が残っているバブルがなければボーナススコアの計算
            CalculateComboBonusScore();
            return;
            
        }

        /// <summary>
        /// ボーナススコアの計算をするメソッド
        /// </summary>
        private void CalculateComboBonusScore()
        {
            //Debug.Log("ボーナススコアの計算");

            _destroyedBubbles.Clear();
            _comboCount = 0;

            return;
        }

    }

}

