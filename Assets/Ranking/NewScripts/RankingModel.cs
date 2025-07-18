#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R3;
using Shabon.Game;
using Shabon.Title;
using UnityEngine.Localization.Settings;

namespace Ranking
{
    /// <summary>
    /// 表示するデータを管理
    /// </summary>
    public class RankingModel
    {
        // 現在表示するリザルトデータの種類
        public ResultEnum CurrentResultType => GetResultEnum(_currentIndex);
        public IEnumerable<ResultDataModel> ResultDataModels => _resultDataModels;
        // リザルトデータを保管するリスト
        private IEnumerable<ResultDataModel> _resultDataModels = Enumerable.Empty<ResultDataModel>();
        // リザルトデータを更新する頻度
        private const float UpdateInterval = 10f;
        // 次のページに映る
        private const float SwipeInterval = 3f;

        // 現在のリザルトデータの種類を示すインデックス(1 ~ Enum数-1)
        private int _currentIndex = 1;
        // 現在の言語設定
        private Language _currentLanguage = Language.Japanese;

        private List<IDisposable> _disposables = new();

        private IDisposable? _updateDisposable;


        public RankingModel()
        {
            // 初期化時にデータを読み込む
            UpdateData();

            // データの更新
            _disposables.Add(
                Observable.Interval(TimeSpan.FromSeconds(UpdateInterval))
                    .Subscribe(_ =>
                    {
                        UpdateData();
                    })
            );

            // インデックスの更新
            UpdateIndex();
        }

        // インデックスを更新(指定もできる)
        public void UpdateIndex(int index = 0)
        {
            _updateDisposable?.Dispose();

            int targetIndex = index;
            // デフォルト処理
            if (index == 0)
            {
                // 言語を切り替えてから更新
                if (_currentLanguage == Language.Japanese)
                {
                    SetLanguage(Language.English);
                    targetIndex = _currentIndex;
                }
                else
                {
                    SetLanguage(Language.Japanese);
                    targetIndex = _currentIndex + 1;
                }
            }
            // はいん以外なら1
            if (targetIndex <= 0 || targetIndex >= System.Enum.GetValues(typeof(ResultEnum)).Length)
            {
                _currentIndex = 1;
            }
            else
            {
                _currentIndex = targetIndex;
            }

            // 次回の更新
            _updateDisposable = Observable.Timer(TimeSpan.FromSeconds(SwipeInterval))
                .Subscribe(_ => UpdateIndex());
        }

        // 言語設定
        public async void SetLanguage(Language language)
        {
            _currentLanguage = language;
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


        // indexからenumを返す
        private ResultEnum GetResultEnum(int index)
        {
            // インデックスが範囲外の場合はNoneを返す
            if (index <= 0 || index >= System.Enum.GetValues(typeof(ResultEnum)).Length)
            {
                return ResultEnum.None;
            }
            return (ResultEnum)index;
        }

        // データを最新のものにする
        private void UpdateData()
        {
            _resultDataModels = ResultData.LoadAllResults();
        }
    }
}