#nullable enable
using UnityEngine;

namespace Shabon.Input
{
    /// <summary>
    /// このゲームの入力はこのクラスを介して行う
    /// </summary>
    public class InputManager : IInputManager
    {
        private readonly SerialInput _serialInput;

        public InputManager()
        {
            _serialInput = new SerialInput();
        }
        // Clapしたときに一度だけtrue
        public bool GetClap()
        {
            return UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space) // スペースをClapの代替
                || (_serialInput.Value1 == 1f); // デバイスの入力
        }

        // Breathの大きさに応じた値0~1を返す
        public float GetBreath()
        {
            float value = (_serialInput.Value0 - 2f) / 8f; // だいたいこれくらい
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Alpha1))
            {
                value = 0.5f;
            }
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Alpha2))
            {
                value = 1f;
            }

            return Mathf.Clamp01(value);
        }

        // 左右の移動量を-1~1で返す
        public float GetHorizontalDirection()
        {
            float value = UnityEngine.Input.GetAxis("Mouse X") / 2f;//カーソルの横の移動量を取得

            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow))
            {
                value = 0.5f;
            }
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow))
            {
                value = -0.5f;
            }


            return value;
        }

        //  メニュー画面を開く入力を取得するメソッド
        public bool GetMenuOpen()
        {
            return UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.M);
        }

        // メニュー画面の決定入力を取得するメソッド
        public bool GetMenuConfirm()
        {
            return UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space);
        }

        // メニュー画面の戻る入力を取得するメソッド
        public bool GetMenuBack()
        {
            return UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.B);
        }

        // 音量の調整する入力を取得するメソッド
        public float GetVolumeAdjustment()
        {
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow))
            {
                return 1f;
            }
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow))
            {
                return -1f;
            }

            return 0f;
        }
    }
}