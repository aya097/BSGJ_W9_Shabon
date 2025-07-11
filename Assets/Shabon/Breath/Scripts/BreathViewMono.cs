#nullable enable
using System.Collections;
using System.Collections.Generic;
using R3;
using Shabon.Bubble;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using VContainer;
namespace Shabon.Breath
{
    /// <summary>
    /// Breathオブジェクトを動かしたり、該当するBubbleを取得するクラス
    /// </summary>
    public class BreathViewMono : MonoBehaviour
    {
        [SerializeField] Transform originTransform = null!; // 原点の位置
        [SerializeField] VisualEffect breathEffect = null!;
        [Inject]
        public void Initialize(BreathModel breath)
        {
            // 座標を初期化
            breath.SetPosition(originTransform.position);

            // 向きが変わったら更新
            Observable.EveryValueChanged(breath, b => b.Direction)
                .Subscribe((direction) =>
                {
                    originTransform.LookAt(breath.Position + direction);
                    // VFXに反映
                    // 向きは変えない
                    breathEffect.transform.rotation = Quaternion.identity;
                    float angle = -originTransform.eulerAngles.y / 180f * Mathf.PI;
                    breathEffect.SetFloat("myAngle", angle);
                    breathEffect.SetFloat("CollisionAngle", angle);
                })
                .AddTo(this);
            // 座標がかわったら更新
            Observable.EveryValueChanged(breath, b => b.Position)
               .Subscribe((position) =>
               {
                   originTransform.position = position;
               })
               .AddTo(this);

            // 息を吹いたら
            Observable.EveryValueChanged(breath, b => b.Strength)
                .Subscribe((strength) =>
                {
                    if (strength > 0)
                    {
                        breathEffect.Play();
                    }
                    else
                    {
                        breathEffect.Stop();
                    }
                })
                .AddTo(this);
        }

    }
}