#nullable enable
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Shabon.Tutorial
{
    public enum TutorialText
    {
        bubble_coming0,
        dirty_mansion,
        bubble_coming1,
        blow_air,
        to_bubble,
        now_blow,
        nice_blow,
        bubble_coming2,
        lure_bubble,
        prepare_hand,
        now_clap,
        clean_mansion,
        thanks,

    }
    /// <summary>
    /// チュートリアルのテキストなどを管理
    /// </summary>
    public class TutorialViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject view = null!;
        [SerializeField] private TMP_Text tutorialText = null!;
        [SerializeField] private LocalizeStringEvent localizeString = null!;
        [SerializeField] private float windowSpeed = 0.5f;

        public void Open()
        {
            LMotion.Create(0f, 1f, windowSpeed)
                .BindToLocalScaleX(view.transform)
                .AddTo(this);

            view.SetActive(true);
        }

        public void Close()
        {
            LMotion.Create(1f, 0f, windowSpeed)
                .WithOnComplete(() => view.SetActive(false))
                .BindToLocalScaleX(view.transform)
                .AddTo(this);

        }

        public void SetText(TutorialText tutorialText)
        {
            // LocalizedString を生成
            var localizedString = new LocalizedString();
            localizedString.TableReference = "TextTable"; // テーブル名
            localizedString.TableEntryReference = "tutorial_" + tutorialText.ToString();   // エントリ名（キー）

            // LocalizeStringEvent に設定
            localizeString.StringReference = localizedString;
            localizeString.RefreshString(); // 手動で更新する
        }
    }
}
