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
        [Header("床のうねりについて")]
        [SerializeField] private bool waveEnable = true;
        [SerializeField] private Material waveMaterial = null!;
        [SerializeField] private GameObject wavedObject = null!;
        [SerializeField] private float waveRadius;
        [SerializeField] private float waveDuration;

        [Header("輪っかについて")]
        [SerializeField] private bool ringEnable = true;

        [SerializeField] private GameObject ringObject = null!;
        [SerializeField] private Ease ringEase;
        [SerializeField] private float ringRadius;
        [SerializeField] private float ringDuration;

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
            if (waveEnable)
            {
                float radiusRatio = waveRadius / wavedObject.transform.localScale.x;
                // 0から半径（全体の大きさに対する割合）まで変化
                LMotion.Create(0f, radiusRatio, waveDuration)
                    .WithEase(Ease.Linear)
                    .WithOnComplete(() => SetWave(0f))
                    .Bind(value => SetWave(value))
                    .AddTo(this);
            }

            if (ringEnable)
            {
                // リングの半径
                LMotion.Create(0f, ringRadius, ringDuration)
                .WithEase(ringEase)
                .WithOnComplete(() => SetRing(0f))
                .Bind(value => SetRing(value))
                .AddTo(this);
                // 色変化
                var ringRenderer = ringObject.GetComponent<SpriteRenderer>();
                LMotion.Create(1f, 0f, ringDuration)
                    .WithEase(Ease.OutQuart)
                    .BindToColorA(ringRenderer)
                    .AddTo(this);
            }
        }

        private void SetWave(float radius)
        {
            waveMaterial.SetFloat("_Radius", radius);
        }

        private void SetRing(float radius)
        {
            ringObject.transform.localScale = Vector3.one * radius;
        }

    }
}