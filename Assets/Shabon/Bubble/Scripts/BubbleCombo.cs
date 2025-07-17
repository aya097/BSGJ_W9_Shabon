#nullable enable

using UnityEngine;
using VContainer.Unity;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルのコンボを管理するクラス
    /// </summary>
    public class BubbleCombo : IBubbleCombo, ITickable
    {
        public int ComboNum => _comboCount;
        public int MaxNum => _comboMax;
        public bool IsCombo => _isCombo;
        public bool IsBossClapped => _isBossClapped;

        private int _comboCount = 0;
        private int _comboMax = 0;
        private bool _isCombo = false;
        private bool _isBossClapped = false;
        private float _comboActiveTime = 0.3f; // コンボ判定は一瞬だと思うので0.1sで設定
        private float _currentTime = 0;

        void ITickable.Tick()
        {
            // comboの有効時間超過時の判定処理
            if (_isCombo)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime >= _comboActiveTime) _isCombo = false;      
            }
            
        }

        /// <summary>
        /// コンボを加算
        /// </summary>
        public void Increase(bool isBossClapped = false)
        {
            // コンボ中でなければ、コンボを開始
            if (_isCombo == false) _isCombo = true;

            _comboCount++;
            if (isBossClapped) _isBossClapped = true;

            // 最大コンボ数の取得
            if (_comboCount >= _comboMax)
            {
                _comboMax = _comboCount;
            }
        }

        /// <summary>
        /// コンボ中の経過時間およびコンボをリセットするメソッド
        /// </summary>
        public void Reset()
        {
            _currentTime = 0;
            _comboCount = 0;
            _isBossClapped = false;
        }
    }

}

