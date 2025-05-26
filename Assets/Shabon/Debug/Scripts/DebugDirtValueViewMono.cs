#nullable enable
using UnityEngine;
using TMPro;
using VContainer;
using Shabon.Score;
using R3;

namespace Shabon.SelfDebug
{
    /// <summary>
    /// DirtValueをデバッグ用UIに表示するクラス
    /// </summary>
    public class DebugDirtValueViewMono : MonoBehaviour
    {
        [SerializeField] TMP_Text dirtText = null!;

        [Inject]
        public void Initialize(IDirtValue dirtValue)
        {
            Observable.EveryValueChanged(dirtValue, d => d.DirtNum)
            .Subscribe(value =>
            {
                dirtText.text = $"DirtValue	: {value}";
            }).AddTo(this);
        }
    }
}
