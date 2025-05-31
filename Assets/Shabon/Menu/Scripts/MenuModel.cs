#nullable enable
using UnityEngine;

namespace Shabon.Menu
{
    // menuの状態
    public enum MenuState
    {
        Game, 
        Pause,
        Controls,
        Option,
        Volume,
        Resolution
    }

    /// <summary>
    /// menuのロジックや状態を管理するクラス
    /// </summary>
    public class MenuModel
    {
        public MenuState CurrentState => _currentState;
        private MenuState _currentState;

        // menuの状態設定を行うメソッド
        public void SetState(MenuState menuState)
        {
            _currentState = menuState;
            UpdateGameState(menuState);
        }

        // ゲームの状態を更新するメソッド
        private void UpdateGameState(MenuState menuState)
        {
            if (menuState == MenuState.Pause)
            {
                Time.timeScale = 0; // ゲームを一時停止
            }
            else if (menuState == MenuState.Game)
            {
                Time.timeScale = 1; // ゲームを再開
            }
        }

        // 一つ前のMenuStateを取得するメソッド
        public MenuState GetPreviousMenuState()
        {
            switch (CurrentState)
            {
                case MenuState.Pause:
                    return MenuState.Game;
                case MenuState.Controls:
                    return MenuState.Pause;
                case MenuState.Option:
                    return MenuState.Pause;
                case MenuState.Volume:
                    return MenuState.Option;
                case MenuState.Resolution:
                    return MenuState.Option;
                default:
                    Debug.LogWarning("不正なMenuStateです。Pauseを返します。");
                    return MenuState.Pause;
            }
        }

    }

}

