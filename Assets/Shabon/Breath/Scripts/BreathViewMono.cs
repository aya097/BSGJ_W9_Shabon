#nullable enable
using R3;
using Shabon.Bubble;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
namespace Shabon.Breath
{
    /// <summary>
    /// Breathオブジェクトを動かしたり、該当するBubbleを取得するクラス
    /// </summary>
    public class BreathViewMono : MonoBehaviour
    {
        [SerializeField] Transform originTransform = null!; // 原点の位置
        [Inject]
        public void Initialize(BreathModel breath)
        {
            originTransform.position = breath.Position;

            // 向きが変わったら更新
            Observable.EveryValueChanged(breath, b => b.Direction)
                .Subscribe((direction) =>
                {
                    originTransform.LookAt(breath.Position + direction);
                })
                .AddTo(this);
            // 座標がかわったら更新
            Observable.EveryValueChanged(breath, b => b.Position)
               .Subscribe((position) =>
               {
                   originTransform.position = position;
               })
               .AddTo(this);
        }
    }
}