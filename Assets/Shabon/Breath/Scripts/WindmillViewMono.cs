#nullable enable
using Shabon.Input;
using UnityEngine;
using VContainer;

namespace Shabon.Breath
{
    /// <summary>
    /// 風車のViewクラス
    /// </summary>
    public class WindmillViewMono : MonoBehaviour
    {
        // 軸部分
        [SerializeField] private GameObject windMillRoot = null!;
        // 回転部分
        [SerializeField] private GameObject windMillWing = null!;
        [Header("Breathの強さが1のとき一秒間に回る角度[deg]")]
        [SerializeField] private float _rotationSpeed;
        private BreathModel _breathModel = null!;
        private IInputManager _inputManager = null!;

        [Inject]

        void Initialize(BreathModel breathModel,
            IInputManager inputManager)
        {
            _breathModel = breathModel;
            _inputManager = inputManager;
        }

        void Update()
        {
            // 向きを更新
            windMillRoot.transform.LookAt(windMillRoot.transform.position + _breathModel.Direction);
            // 回転
            windMillWing.transform.Rotate(new Vector3(_inputManager.GetBreath() * Time.deltaTime * _rotationSpeed, 0, 0));
        }
    }
}