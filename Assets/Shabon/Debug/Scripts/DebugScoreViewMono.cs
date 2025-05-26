#nullable enable
using UnityEngine;
using TMPro;
using VContainer;
using R3;
using Shabon.Bubble;
using Shabon.Score;

namespace Shabon.SelfDebug
{
    /// <summary>
    /// Scoreをデバッグ用UIに表示するクラス
    /// </summary>
    public class DebugScoreViewMono : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText = null!;

        [Inject]
        public void Initialize(IScoreValue scoreValue)
        {
            Observable.EveryValueChanged(scoreValue, b => b.ScoreNum)
            .Subscribe(value =>
            {
                scoreText.text = $"Score		: {value}";
            }).AddTo(this);
        }
    }
}
