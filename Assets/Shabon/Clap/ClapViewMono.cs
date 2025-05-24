#nullable enable
using UnityEngine;
using VContainer;

namespace Shabon.Clap
{
    public class ClapViewMono : MonoBehaviour
    {
        private ClapModel _clapModel = null!;
        private ClapGetterViewMono? clapGetter;

        [Inject]
        public void Initialize(ClapModel clapModel)
        {
            _clapModel = clapModel;
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
    }
}

