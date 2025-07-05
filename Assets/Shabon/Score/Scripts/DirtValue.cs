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

        public int DecreaseCount { get; private set; } = 0;
        public int ClapDecreaseCount { get; private set; } = 0;

        // 引数は正の数
        public DirtValue()
        {
            _dirtNum = 0;
        }

        public int TotalIncrease { get; private set; } = 0; // 累計増加量
        public int TotalDecrease { get; private set; } = 0; // 累計減少量

        // 引数は正の数
        public void Increase(int value)
        {
            if (IsAssertMinusNum(value)) return;
            _dirtNum += value;
            TotalIncrease += value; // 増加量を加算
        }



        // Clapによる減少
        public void DecreaseByClap(int value)
        {
            if (IsAssertMinusNum(value)) return;
            int before = _dirtNum;
            _dirtNum -= value;
            int actualDecrease = Mathf.Min(before, value);
            TotalDecrease += actualDecrease;
            DecreaseCount++;
            ClapDecreaseCount++; // Clapによる減少回数を加算
            if (_dirtNum < 0)
            {
                _dirtNum = 0;
            }
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