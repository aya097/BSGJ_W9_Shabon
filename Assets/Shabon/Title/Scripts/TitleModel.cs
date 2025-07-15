#nullable enable

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

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

        private Language _currentLanguage = Language.Japanese;

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
            if (language != _currentLanguage)
            {
                _currentLanguage = language;
                Debug.Log(language);
                _ = ChangeLocale(language);
            }
        }

        // 言語確定
        public void DecideLanguage()
        {
            _currentState = TitleState.Prologue;
        }

        // 言語切り替える
        private async Task ChangeLocale(Language language)
        {
            string key = "";
            if (language == Language.English)
            {
                key = "en";
            }
            else if (language == Language.Japanese)
            {
                key = "ja";
            }
            // 指定したLocaleを取得
            var locale = LocalizationSettings.AvailableLocales.Locales.Find((x) => x.Identifier.Code == key);

            // Localeの設定
            LocalizationSettings.SelectedLocale = locale;

            // 初期化待ち
            await LocalizationSettings.InitializationOperation.Task;
        }

    }
}