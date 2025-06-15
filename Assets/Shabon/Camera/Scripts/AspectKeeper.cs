#nullable enable
using UnityEngine;

namespace Shabon.SelfCamera
{
    /// <summary>
    /// 水平視野を合わせる機能
    /// </summary>
    public class AspectKeeper : MonoBehaviour
    {
        public Camera _camera;
        public float baseWidth = 16.0f;
        public float baseHeight = 9.0f;

        void Update()
        {
            // アスペクト比固定
            var scale = Mathf.Min(Screen.height / this.baseHeight, Screen.width / this.baseWidth);
            var width = baseWidth * scale / Screen.width;
            var height = baseHeight * scale / Screen.height;
            _camera.rect = new Rect((1.0f - width) * 0.5f, (1.0f - height) * 0.5f, width, height);
        }
    }
}