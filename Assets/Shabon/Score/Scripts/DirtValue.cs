#nullable enable

using UnityEngine;

namespace Shabon.Score
{
    /// <summary>
    /// スコアの値を扱うクラス、正の数のみ扱う
    /// </summary>
    public class DirtValue : IDirtValue
    {
        public int DirtNum
        {
            get { return _dirtNum; }
        }

        private int _dirtNum;

        // 引数は正の数
        public DirtValue(int value = 0)
        {
            if (IsAssertMinusNum(value))
            {
                _dirtNum = 0;
            }
            else
            {
                _dirtNum = value;
            }
        }

        // 引数は正の数
        public void Increase(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _dirtNum += value;
        }

        // 引数は正の数
        public void Decrease(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _dirtNum -= value;
        }

        // 引数は正の数
        public void Set(int value)
        {
            if (IsAssertMinusNum(value)) return;

            _dirtNum = value;
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