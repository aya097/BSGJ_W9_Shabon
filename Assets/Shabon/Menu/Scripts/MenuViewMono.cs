using UnityEngine;
using UnityEngine.UI;
using System;
using VContainer;
using R3;

/// <summary>
/// メニュー画面のUIを管理するクラス
/// </summary>
public class MenuViewMono : MonoBehaviour
{
    // Pause Canvas
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private Button _backGameButton;
    [SerializeField] private Button _controlsButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _backTitleButton;

    // Option Canvas
    [SerializeField] private Canvas _optionCanvas;
    [SerializeField] private Button _volumeButton;
    [SerializeField] private Button _resolutionButton;

    // Volume Canvas
    [SerializeField] private Canvas _volumeCanvas;

    // Resolution Canvas
    [SerializeField] private Canvas _resolutionCanvas;


    public event Action OnBackGameButtonClicked;
    public event Action OnControlsButtonClicked;
    public event Action OnOptionButtonClicked;
    public event Action OnBackTitleButtonClicked;

    public event Action OnVolumeButtonClicked;
    public event Action OnResolutionButtonClicked;

    [Inject]
    public void Initialize()
    {
        // ボタンのクリックイベントを登録
        _backGameButton.OnClickAsObservable()
        .Subscribe(_ => OnBackGameButtonClicked.Invoke())
        .AddTo(this);

        _controlsButton.OnClickAsObservable()
        .Subscribe(_ => OnControlsButtonClicked.Invoke())
        .AddTo(this);

        _optionButton.OnClickAsObservable()
        .Subscribe(_ => OnOptionButtonClicked.Invoke())
        .AddTo(this);

        _backTitleButton.OnClickAsObservable()
        .Subscribe(_ => OnBackTitleButtonClicked.Invoke())
        .AddTo(this);

        _volumeButton.OnClickAsObservable()
        .Subscribe(_ => OnVolumeButtonClicked.Invoke())
        .AddTo(this);

        _resolutionButton.OnClickAsObservable()
        .Subscribe(_ => OnResolutionButtonClicked.Invoke())
        .AddTo(this);

    }

    // 指定したcanvasを表示するメソッド
    public void ShowCanvas(Canvas canvasToShow)
    {
        // 全てのキャンバスを非表示にする
        _pauseCanvas.gameObject.SetActive(false);
        _volumeCanvas.gameObject.SetActive(false);
        _resolutionCanvas.gameObject.SetActive(false);

        // 指定したキャンバスを表示する
        canvasToShow.gameObject.SetActive(true);
    }
}
