using UnityEngine;
using UnityEngine.UI;
using System;
using VContainer;
using R3;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Shabon.Menu
{
    public enum ResolutionState
    {
        High,
        Middle,
        Low
    }

    /// <summary>
    /// メニュー画面のUIを管理するクラス
    /// </summary>
    public class MenuViewMono : MonoBehaviour
    {
        [SerializeField] private List<MenuStatePanelPair> _menuStatePanelPairs;

        // Pause Canvas
        [Header("ポーズ画面")]
        [SerializeField] private Button _backGameButton;
        [SerializeField] private Button _controlsButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _backTitleButton;


        // Option Canvas
        [Header("オプション画面")]
        [SerializeField] private Button _volumeButton;
        [SerializeField] private Button _resolutionButton;


        // Resolution Canvas
        [Header("解像度設定画面")]
        [SerializeField] private Button _highResolutionButton;
        [SerializeField] private Button _middleResolutionButton;
        [SerializeField] private Button _lowResolutionButton;


        public event Action OnBackTitleButtonClicked;
        public event Action<MenuState> OnPanelSwitchButtonClicked;
        public event Action<ResolutionState> OnChangeResolutionButtonClicked;


        [Inject]
        public void Initialize()
        {
            // ポーズ画面のUIのボタン
            _backGameButton.OnClickAsObservable()
                .Subscribe(_ => OnPanelSwitchButtonClicked.Invoke(MenuState.Game))
                .AddTo(this);

            _backTitleButton.OnClickAsObservable()
                .Subscribe(_ => OnBackTitleButtonClicked.Invoke())
                .AddTo(this);


            // panel切り替えのボタン
            _controlsButton.OnClickAsObservable()
                .Subscribe(_ => OnPanelSwitchButtonClicked.Invoke(MenuState.Controls))
                .AddTo(this);

            _optionButton.OnClickAsObservable()
                .Subscribe(_ => OnPanelSwitchButtonClicked.Invoke(MenuState.Option))
                .AddTo(this);

            _volumeButton.OnClickAsObservable()
                .Subscribe(_ => OnPanelSwitchButtonClicked.Invoke(MenuState.Volume))
                .AddTo(this);

            _resolutionButton.OnClickAsObservable()
                .Subscribe(_ => OnPanelSwitchButtonClicked.Invoke(MenuState.Resolution))
                .AddTo(this);

            // 解像度画面のボタン
            _highResolutionButton.OnClickAsObservable()
                .Subscribe(_ => OnChangeResolutionButtonClicked.Invoke(ResolutionState.High))
                .AddTo(this);

            _middleResolutionButton.OnClickAsObservable()
                .Subscribe(_ => OnChangeResolutionButtonClicked.Invoke(ResolutionState.Middle))
                .AddTo(this);

            _lowResolutionButton.OnClickAsObservable()
                .Subscribe(_ => OnChangeResolutionButtonClicked.Invoke(ResolutionState.Low))
                .AddTo(this);

        }

        // 指定したmenuStateに対応するpanelを表示するメソッド
        public void SwitchPanel(MenuState menuState)
        {
            foreach (MenuStatePanelPair menuStatePanelPair in _menuStatePanelPairs)
            {
                // panelを非表示
                menuStatePanelPair.panel.SetActive(false);

                // 指定されたpanelの設定
                if (menuStatePanelPair.menuState == menuState)
                {
                    // panelの表示
                    menuStatePanelPair.panel.SetActive(true);

                    // panel上で初期選択されるUIを設定
                    Selectable[] childObjects = menuStatePanelPair.panel.GetComponentsInChildren<Selectable>(true);
                    Selectable selectableObject = childObjects.FirstOrDefault(b => b.gameObject.activeInHierarchy && b.interactable);
                    if (selectableObject != null)
                        EventSystem.current.SetSelectedGameObject(selectableObject.gameObject);
                }
            }

        }
        
        // 指定した解像度に更新するメソッド
        public void UpdateResolutionDisplay(ResolutionState resolutionState)
        {
            switch (resolutionState)
            {
                case ResolutionState.High:
                    Debug.Log("解像度を 高 にしました");
                    return;
                case ResolutionState.Middle:
                    Debug.Log("解像度を 中 にしました");
                    return;
                case ResolutionState.Low:
                    Debug.Log("解像度を 低 にしました");
                    return;
                default:
                    Debug.LogWarning("不正なResolutionStateです。");
                    break;
            }
        }
    }
}

