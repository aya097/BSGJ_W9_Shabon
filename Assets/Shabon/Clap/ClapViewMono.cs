using UnityEngine;
using System.Linq;
using VContainer;
using Shabon.Input;

namespace Shabon.Clap
{
    public class ClapViewMono : MonoBehaviour
    {
        private ClapModel _clapModel = null!;
        private IInputManager _inputManager = null!;
        [SerializeField] ClapGetterViewMono clapGetter = null!;

        [Inject]
        public void Initialize(ClapModel clapModel, IInputManager inputManager)
        {
            _clapModel = clapModel;
            _inputManager = inputManager;
        }

        void Awake()
        {
            // 自動的にClapGetterViewMonoを取得
            if (clapGetter == null)
            {
                clapGetter = GetComponentInChildren<ClapGetterViewMono>();

            }
        }

        public void ExecuteClap(float strength)
        {
            _clapModel.ExecuteClap(strength);
        }

        void Update()
        {
            if (_clapModel == null)
            {
                Debug.LogError("ClapModel is null. Ensure it is properly injected.");
                return;
            }

            if (clapGetter == null)
            {
                Debug.LogError("ClapGetterViewMono is not assigned in the inspector.");
                return;
            }

            if (_inputManager.GetClap())
            {
                var bubbles = clapGetter.GetBubbleMonos();
                if (bubbles != null && bubbles.Any())
                {
                    _clapModel.ExecuteClap(1f);
                }
            }
        }
    }
}
