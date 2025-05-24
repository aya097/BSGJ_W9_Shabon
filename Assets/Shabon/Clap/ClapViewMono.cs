#nullable enable
using UnityEngine;
using VContainer;

namespace Shabon.Clap
{
    public class ClapViewMono : MonoBehaviour
    {
        [Inject] ClapModel _clapModel = null!;
        [Inject] ClapGetterViewMono? clapGetter;

        [Inject]
        public void Initialize(ClapModel clapModel)
        {
            _clapModel = clapModel;
        }

        void Awake()
        {

            if (clapGetter == null)
            {
                clapGetter = GetComponentInChildren<ClapGetterViewMono>();
            }
        }

        public void ExecuteClap(float strength)
        {
            _clapModel.ExecuteClap(strength);
        }
    }
}

