using UnityEngine;
using System.Linq;

namespace Shabon.Clap
{
    public class ClapViewMono : MonoBehaviour
    {
        [SerializeField] ClapGetterViewMono clapGetter = null!;
        [SerializeField] ClapModel clapModel = null!;

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) // シフトキーが押されたとき
            {
                var bubbles = clapGetter.GetBubbleMonos(); // ClapGetterViewMono を使用して範囲内のバブルを取得
                if (bubbles != null && bubbles.Any()) // Any メソッドを使用
                {
                    clapModel.ExecuteClap(1f); // ClapModel を使用して範囲内の Bubble を消滅
                }
            }
        }
    }
}
