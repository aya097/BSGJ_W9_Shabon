#nullable enable
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
