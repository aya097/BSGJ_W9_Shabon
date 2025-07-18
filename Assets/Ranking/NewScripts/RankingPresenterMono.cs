#nullable enable
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Ranking
{
    public class RankingPresenterMono : MonoBehaviour
    {
        // Model
        private RankingModel _rankingModel = null!;

        // View
        [SerializeField] private RankingViewSelectorMono rankingView = null!;


        private bool _isInverse = false;
        private void OnDestroy()
        {
            _rankingModel.Dispose();
        }

        void Awake()
        {
            _rankingModel = new RankingModel();
        }

        void Start()
        {
            // Model -> View
            // データが変わる
            Observable.EveryValueChanged(_rankingModel, r => r.CurrentResultType)
                .Subscribe(value =>
                {
                    var texts = GetRanking(value);
                    rankingView.SetTexts(GetRanking(value), value, _isInverse);
                    _isInverse = false;
                })
                .AddTo(this);

        }

        void Update()
        {
            if (rankingView.IsAvailable == false)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // 右キーで次のページへ
                _rankingModel.UpdateIndex(_rankingModel.CurrentIndex + 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // 左キーで前のページへ
                int targetIndex = _rankingModel.CurrentIndex - 1;
                if (targetIndex < 1)
                {
                    targetIndex = System.Enum.GetValues(typeof(ResultEnum)).Length - 1; // 最後のページへ
                }
                _rankingModel.UpdateIndex(targetIndex);
                _isInverse = true;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _rankingModel.SetLanguage(Shabon.Title.Language.Japanese);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _rankingModel.SetLanguage(Shabon.Title.Language.English);
            }
        }

        private IEnumerable<string> GetRanking(ResultEnum resultEnum)
        {
            if (resultEnum == ResultEnum.Dirt)
            {
                // Dirt 小さい順
                var str = _rankingModel.ResultDataModels.OrderBy(r => r.FinalDirt).Take(5).Select(r => r.FinalDirt.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.Score)
            {
                // Score 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.FinalScore).Take(5).Select(r => r.FinalScore.ToString());
                return str;

            }
            else if (resultEnum == ResultEnum.Combo)
            {
                // Combo 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.FinalCombo).Take(5).Select(r => r.FinalCombo.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.Clap)
            {
                // ClapCount 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.FinalClapCount).Take(5).Select(r => r.FinalClapCount.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.SumDirt)
            {
                // SumDirt 小さい順
                var str = _rankingModel.ResultDataModels.OrderBy(r => r.DirtValueCountSum).Take(5).Select(r => r.DirtValueCountSum.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.Breath)
            {
                // BreathTime 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.FinalBreathTime).Take(5).Select(r => r.FinalBreathTime.ToString("F1"));
                return str;
            }
            else if (resultEnum == ResultEnum.Calorie)
            {
                // Calorie 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.Calorie).Take(5).Select(r => r.Calorie.ToString("F1"));
                return str;
            }
            else if (resultEnum == ResultEnum.Boss)
            {
                // BossBattleTime 小さい順
                var str = _rankingModel.ResultDataModels.Where(r => r.BossBattleTime != -1).OrderBy(r => r.BossBattleTime).Take(5).Select(r => r.BossBattleTime.ToString("F1"));
                return str;
            }

            return Enumerable.Empty<string>();
        }

    }
}