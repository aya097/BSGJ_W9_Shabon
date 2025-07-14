#nullable enable

namespace Shabon.Title
{
    public enum TitleState
    {
        None,
        Start,
        Language,
        Prologue,
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
        public void SetLanguage()
        {

        }

        // プロローグスタート
        public void StartPrologue()
        {
            _currentState = TitleState.Prologue;
        }

    }
}