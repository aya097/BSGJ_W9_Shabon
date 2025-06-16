#nullable enable

using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEngine;
using VContainer;

namespace Shabon.Clap
{
    /// <summary>
    /// Clapのエフェクトを実装するクラス
    /// </summary>
    public class ClapViewMono : MonoBehaviour
    {
        [SerializeField] private Material waveMaterial = null!;
        [SerializeField] private GameObject wavedObject = null!;
        [SerializeField] private float waveRadius;
        [SerializeField] private float duration;


        [Inject]
        public void Initialize(ClapModel clapModel)
        {
            // クラップされたら再生
            Observable.EveryValueChanged(clapModel, c => c.IsClap)
                .Subscribe(isClap =>
                {
                    if (isClap)
                    {
                        Play();
                    }
                }).AddTo(this);
        }

        private void Play()
        {
            float radiusRatio = waveRadius / wavedObject.transform.localScale.x;
            // 0から半径（全体の大きさに対する割合）まで変化
            LMotion.Create(0f, radiusRatio, duration)
                .WithEase(Ease.Linear)
                .WithOnComplete(() => SetRadius(0f))
                .Bind(value => SetRadius(value))
                .AddTo(this);
        }

        private void SetRadius(float radius)
        {
            waveMaterial.SetFloat("_Radius", radius);
        }

    }
}