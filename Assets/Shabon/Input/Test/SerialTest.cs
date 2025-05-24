#nullable enable
using UnityEngine;

namespace Shabon.Input
{
    public class SerialTest : MonoBehaviour
    {
        [SerializeField] Transform root = null!;
        SerialInput _serialInput = null!;
        void Start()
        {
            _serialInput = new SerialInput();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(_serialInput.Value);

            root.Rotate(0, 0, _serialInput.Value * 180 * Time.deltaTime);
        }
    }
}
