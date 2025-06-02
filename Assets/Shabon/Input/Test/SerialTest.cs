#nullable enable
using R3;
using UnityEngine;

namespace Shabon.Input
{
    public class SerialTest : MonoBehaviour
    {
        [SerializeField] Transform root = null!;
        SerialInput _serialInput = null!;

        float _clapValueBuffer = 0;
        void Start()
        {
            _serialInput = new SerialInput();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(_serialInput.Value0);

            root.Rotate(0, 0, _serialInput.Value0 * 180 * Time.deltaTime);

            if (_serialInput.Value1 != _clapValueBuffer)
            {
                _clapValueBuffer = _serialInput.Value1;
                if (_serialInput.Value1 == 1) Debug.Log("Clapppppppppp!");
            }
        }
    }
}
