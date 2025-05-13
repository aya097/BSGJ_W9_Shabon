using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Breath
{
    public class Breath : MonoBehaviour
    {

        private BubbleMover bubbleMover;




        void Update()
        //十字キーでObjectを移動させる
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Translate(-0.02f, 0.0f, 0.0f);
            }
            // 右に移動
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Translate(0.02f, 0.0f, 0.0f);
            }
        }

    }
}
