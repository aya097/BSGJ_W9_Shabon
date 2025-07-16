#nullable enable
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace Shabon.Title
{
    /// <summary>
    /// 背景を拡大したり、エフェクトかけたり
    /// </summary>
    public class SelectLanguageViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject logoView = null!;
        [SerializeField] private GameObject startView = null!;
        [SerializeField] private GameObject filterView = null!;
        [SerializeField] private TMP_Text japaneseText = null!;
        [SerializeField] private TMP_Text englishText = null!;

        [SerializeField] private float awayTime;
        [SerializeField] private Ease awayEase;

        public void Open()
        {
            // ロゴをふっとばす
            var logoTransform = logoView.GetComponent<RectTransform>();
            LMotion.Create(logoTransform.anchoredPosition.y, 1000, awayTime)
                .WithEase(awayEase)
                .BindToAnchoredPositionY(logoTransform)
                .AddTo(this);
            // スタートをふっとばす
            var startTransform = startView.GetComponent<RectTransform>();
            LMotion.Create(startTransform.anchoredPosition.y, -1000, awayTime)
                .WithEase(awayEase)
                .BindToAnchoredPositionY(startTransform)
                .AddTo(this);
            // フィルタ大きくする
            var filterTransform = filterView.GetComponent<RectTransform>();
            LMotion.Create(Vector3.zero, Vector3.one * 3, awayTime)
                .WithEase(awayEase)
                .WithOnComplete(() =>
                {
                    japaneseText.gameObject.SetActive(true);
                    englishText.gameObject.SetActive(true);
                })
                .BindToLocalScale(filterTransform)
                .AddTo(this);
        }
    }
}