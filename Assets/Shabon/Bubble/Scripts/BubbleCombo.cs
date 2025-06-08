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
        private int _comboCount = 0;

        /// <summary>
        /// コンボを加算
        /// </summary>
        public void Increase()
        {
            _comboCount++;
        }

        /// <summary>
        /// コンボをリセットするメソッド
        /// </summary>
        public void Reset()
        {
            _comboCount = 0;
        }
    }

}

