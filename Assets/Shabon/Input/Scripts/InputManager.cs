#nullable enable
namespace Shabon.Input
{
    /// <summary>
    /// このゲームの入力はこのクラスを介して行う
    /// </summary>
    public class InputManager : IInputManager
    {
        // Clapしたときに一度だけtrue
        public bool GetClap()
        {
            return UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space); // スペースをClapの代替
        }

        // Breathの大きさに応じた値0~1を返す
        public float GetBreath()
        {
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Alpha1))
            {
                return 0.5f;
            }
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Alpha2))
            {
                return 1f;
            }

            return 0;
        }

        // 左右の移動量を-1~1で返す
        public float GetHorizontalDirection()
        {
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow))
            {
                return 1f;
            }
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow))
            {
                return -1f;
            }

            return 0f;
        }
    }
}