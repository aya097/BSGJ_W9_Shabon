#nullable enable

using UnityEngine;

namespace Shabon.Score
{
    /// <summary>
    /// スコアの値を扱うクラス、正の数のみ扱う
    /// </summary>
    public class Score : IScore
    {
        public int ScoreNum { get; private set; }

        private int _scoreNum;

        // 引数は正の数
        public Score(int value = 0)
        {
            if (IsAssertMinusNum(value))
            {
                _scoreNum = 0;
            }
            else
            {
                _scoreNum = value;
            }
        }

        // 引数は正の数
        public void Increase(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _scoreNum += value;
        }

        // 引数は正の数
        public void Decrease(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _scoreNum -= value;
        }

        // 引数は正の数
        public void Set(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _scoreNum = value;
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