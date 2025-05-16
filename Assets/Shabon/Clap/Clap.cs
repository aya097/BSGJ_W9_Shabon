using UnityEngine;

namespace Shabon.Clap
{
    // todo monoなくす
    public class Clap : MonoBehaviour
    {

        // todo IBubbleHandlerのClapを呼ぶ



        void Update()
        //Objectのx座標が２０から−２０の間の時にキーボードのcを押すとObjectが消滅する
        {
            if (transform.position.x >= -20f && transform.position.x <= 20f)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.C))
                {
                    Destroy(this.gameObject);

                }

            }

        }
    }
}