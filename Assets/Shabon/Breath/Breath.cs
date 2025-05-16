using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Breath
{
    // todo MonoBehaviourをなくす
    public class Breath : MonoBehaviour
    {
        // private BubbleCluster _bubbleCluster;
        // private BubbleMover _bubbleMover;

        // todo IBubbleHandlerのBreathを呼ぶ



        void Update()
        //十字キーでObjectを移動させる
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Translate(-0.02f, 0.0f, 0.0f);
            }
            // 右に移動
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Translate(0.02f, 0.0f, 0.0f);
            }
        }

    }
}
