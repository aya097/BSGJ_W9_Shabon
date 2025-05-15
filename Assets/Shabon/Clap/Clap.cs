using UnityEngine;

namespace Shabon.Clap
{
    public class Clap : MonoBehaviour
    {




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