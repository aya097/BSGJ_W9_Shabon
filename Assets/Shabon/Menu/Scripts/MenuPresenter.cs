#nullable enable
using Shabon.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shabon.Menu 
{
    /// <summary>
    /// MenuViewMonoとMenuModelの仲介をするクラス
    /// </summary>
    public class MenuPresenter : ITickable
    {
        private readonly MenuViewMono _menuViewMono;
        private readonly MenuModel _menuModel;
        private readonly IInputManager _inputManager;

        [Inject]
        public MenuPresenter(
            MenuViewMono menuViewMono,
            MenuModel menuModel,
            IInputManager inputManager
        )
        {
            _menuViewMono = menuViewMono;
            _menuModel = menuModel;
            _inputManager = inputManager;

            // View のイベントを購読
            _menuViewMono.OnBackTitleButtonClicked += HandleBackTitleButtonClicked;
            _menuViewMono.OnPanelSwitchButtonClicked += HandlePanelSwitchButtonClicked;
            _menuViewMono.OnChangeResolutionButtonClicked += HandleChangeResolutionButtonClicked;

        }

        void ITickable.Tick()
        {
            if (_menuModel.CurrentState == MenuState.Game)
            {
                HandleGameState();
            }
            else
            {
                HandleMenuState();
            }
        }

        // game中の処理を行うメソッド
        private void HandleGameState()
        {
            if (_inputManager.GetMenuOpen())
            {
                _menuModel.SetState(MenuState.Pause);
                _menuViewMono.SwitchPanel(MenuState.Pause);
            }
        }

        // menu中の処理を行うメソッド
        private void HandleMenuState()
        {
            // ボタン押下の処理
            if (_inputManager.GetMenuConfirm())
            {
                GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject.TryGetComponent<Button>(out Button targetButton))
                    targetButton.onClick?.Invoke();

            }
            // 前のpanelに戻る処理
            else if (_inputManager.GetMenuBack() && _menuModel.CurrentState != MenuState.Pause)
            {
                MenuState previousMenuState = _menuModel.GetPreviousMenuState();
                _menuViewMono.SwitchPanel(previousMenuState);
                _menuModel.SetState(previousMenuState);
            }
        }

        // タイトルに戻るボタンが押下されたときに実行するメソッド
        private void HandleBackTitleButtonClicked()
        {
            Debug.Log("titleに戻ります");
        }

        // panelを切り替えるボタンが押下されたときに実行するメソッド
        private void HandlePanelSwitchButtonClicked(MenuState menuState)
        {
            _menuViewMono.SwitchPanel(menuState);
            _menuModel.SetState(menuState);
        }

        // 解像度を変更するボタンが押下された時に実行するメソッド
        private void HandleChangeResolutionButtonClicked(ResolutionState resolutionState)
        {
            _menuViewMono.UpdateResolutionDisplay(resolutionState);
        }

    }
}

