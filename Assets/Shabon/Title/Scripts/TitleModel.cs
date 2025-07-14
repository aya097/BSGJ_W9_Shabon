#nullable enable

using UnityEngine;

namespace Shabon.Title
{
    public enum TitleState
    {
        None,
        Start,
        Language,
        Prologue,
    }
    public enum Language
    {
        Japanese,
        English,
    }
    /// <summary>
    /// タイトルの状態を管理するクラス
    /// </summary>
    public class TitleModel
    {
        public TitleState CurrentState => _currentState;

        private TitleState _currentState = TitleState.None;

        public TitleModel()
        {
            _currentState = TitleState.Start;
        }


        // ゲームをスタートする
        public void StartGame()
        {
            _currentState = TitleState.Language;
        }

        // 言語を設定する
        public void SetLanguage(Language language)
        {
            Debug.Log(language);
        }

        // 言語確定
        public void DecideLanguage()
        {
            _currentState = TitleState.Prologue;
        }

    }
}