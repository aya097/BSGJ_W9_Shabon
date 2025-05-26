#nullable enable

using System.Collections.Generic;
using System.Linq;
using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルのコンボを管理するクラス
    /// </summary>
    public class BubbleCombo : IBubbleCombo
    {
        public int ComboNum => _comboCount;
        private readonly List<IBubbleMono> _destroyedBubbles = new();
        private readonly List<IBubbleMono> _remainingChainBubbles = new();
        private int _comboCount = 0;
        private readonly IScoreValue _scoreValue;
        private readonly GameRuleParam _gameRuleParam;

        [Inject]
        public BubbleCombo(IScoreValue scoreValue, GameRuleParam gameRuleParam)
        {
            _scoreValue = scoreValue;
            _gameRuleParam = gameRuleParam;
        }

        /// <summary>
        /// destroyしたbubbleをコンボを加算するメソッド
        /// </summary>
        /// <param name="bubbleMono"></param>
        public void AddComboCount(IBubbleMono bubbleMono)
        {
            _destroyedBubbles.Add(bubbleMono);
            _comboCount++;

            RemoveChainedBubble(bubbleMono);
        }

        /// <summary>
        /// 連鎖済みのbubbleをListから除去
        /// </summary>
        /// <param name="bubbleMono"></param>
        public void RemoveChainedBubble(IBubbleMono bubbleMono)
        {
            // 1コンボ目のみ例外処理
            if (_remainingChainBubbles.Count() == 0) return;
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

            // 連鎖が残っているときはボーナススコアの計算しない
            if (_remainingChainBubbles.Count() > 0) return;

            // 破壊されたBubbleが複数個のときコンボボーナスを計算
            if (_destroyedBubbles.Count() > 1) CalculateComboBonusScore();
            ResetCombo();
            return;
        }

        /// <summary>
        /// ボーナススコアの計算をするメソッド
        /// </summary>
        private void CalculateComboBonusScore()
        {
            float sumBubbleScore = _destroyedBubbles.Sum(b => b.BubbleScore);
            float bubbleCount = _destroyedBubbles.Count();

            // コンボボーナススコア = (バブルの総得点 × a) + (バブルの数 × b) 
            // a, bはパラメータ
            int comboBonusScore
                    = (int)(sumBubbleScore * _gameRuleParam.SumBubbleScoreMultiplier
                        + bubbleCount * _gameRuleParam.BubbleCountScoreMultiplier);

            if (comboBonusScore >= 0)
            {
                _scoreValue.Increase(comboBonusScore);
            }
            else
            {
                _scoreValue.Decrease(Mathf.Abs(comboBonusScore));
            }

            return;
        }

        /// <summary>
        /// コンボをリセットするメソッド
        /// </summary>
        private void ResetCombo()
        {
            _destroyedBubbles.Clear();
            _comboCount = 0;
        }


    }

}

