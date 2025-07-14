#nullable enable

using UnityEngine;

namespace Shabon.Score
{
    /// <summary>
    /// スコアの値を扱うクラス、正の数のみ扱う
    /// </summary>
    public class ScoreValue : IScoreValue
    {
        public int ScoreNum
        {
            get { return _scoreNum; }
        }
        public bool IsIncrease
        {
            get { return _isIncrease; }
        }

        private int _scoreNum;
        private bool _isIncrease;

        // 引数は正の数
        public ScoreValue(int value = 0)
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
            _isIncrease = true;
        }

        // 引数は正の数
        public void Decrease(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _scoreNum -= value;
            _isIncrease = false;
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