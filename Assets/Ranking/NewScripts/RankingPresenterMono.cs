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
                    Debug.Log($"RankingPresenterMono: {value} - {string.Join(", ", texts)}");
                    rankingView.SetTexts(GetRanking(value));
                })
                .AddTo(this);
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
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.FinalBreathTime).Take(5).Select(r => r.FinalBreathTime.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.Calorie)
            {
                // Calorie 大きい順
                var str = _rankingModel.ResultDataModels.OrderByDescending(r => r.Calorie).Take(5).Select(r => r.Calorie.ToString());
                return str;
            }
            else if (resultEnum == ResultEnum.Boss)
            {
                // BossBattleTime 小さい順
                var str = _rankingModel.ResultDataModels.OrderBy(r => r.BossBattleTime).Take(5).Select(r => r.BossBattleTime.ToString());
                return str;
            }

            return Enumerable.Empty<string>();
        }

    }
}