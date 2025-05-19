#nullable enable
using R3;
using UnityEngine;
using UnityEngine.UIElements;
namespace Shabon.Breath
{
    public class BreathViewMono : MonoBehaviour
    {
        [SerializeField] Transform originTransform; // 原点の位置 
        public void Initialize(Breath breath)
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