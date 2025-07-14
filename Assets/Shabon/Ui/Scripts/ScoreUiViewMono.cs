#nullable enable
using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UnityEngine;

namespace Shabon.Ui
{
    public class ScoreUiViewMono : MonoBehaviour
    {
        [SerializeField] GameObject view = null!;
        [SerializeField] TMP_Text scoreText = null!;
        [SerializeField] Color increaseColor;
        [SerializeField] Color decreaseColor;
        [SerializeField] float popTime;
        [SerializeField] float remainTime;
        [SerializeField] float returnTime;
        [SerializeField] Ease popEase;

        private Color _originalColor;
        private float _originalSize;

        private List<IDisposable?> _disposables = new();

        void Awake()
        {
            _originalColor = scoreText.color;
            _originalSize = scoreText.fontSize;
        }

        public void Close()
        {
            view.SetActive(false);
        }

        public void SetScore(int score, bool isUp)
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            // 初期化前は実行しない
            if (_originalSize == 0)
            {
                scoreText.text = score.ToString();
                return;
            }

            Color usedColor;
            if (isUp)
            {
                usedColor = increaseColor;
            }
            else
            {
                usedColor = decreaseColor;
            }
            // 一度大きくなってスコアと色更新
            _disposables.Add(
                LMotion.Create(1, 2f, popTime)
                .WithEase(popEase)
                .WithLoops(2, LoopType.Flip)
                .BindToLocalScaleY(scoreText.rectTransform)
                .AddTo(this)
                .ToDisposable()
            );
            _disposables.Add(
                LMotion.Create(_originalColor, usedColor, popTime)
                .WithEase(popEase)
                .BindToColor(scoreText)
                .AddTo(this).
                ToDisposable()
            );

            scoreText.text = score.ToString();


            // 徐々に色を戻す
            _disposables.Add(
                LMotion.Create(usedColor, _originalColor, returnTime)
                .WithDelay(remainTime)
                .BindToColor(scoreText)
                .AddTo(this)
                .ToDisposable()
            );
        }
    }
}