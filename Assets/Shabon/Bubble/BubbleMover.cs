using Shabon.Bubble;
using UnityEngine;


namespace Shabon.Bubble
{
    public class BubbleMover : MonoBehaviour
    {

        public float moveSpeed = -5f;



        void Update()
        {
            // 基本的な移動処理 (Z軸方向に移動)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }



        public void Move(Vector3 movement)
        {
            transform.Translate(movement * Time.deltaTime);
        }

    }


}
