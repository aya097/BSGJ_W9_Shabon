using UnityEngine;

namespace Shabon.Sound
{
    public class SoundTestMono : MonoBehaviour
    {
        SoundToken a, b, c, d;

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                a = SoundPlayerMono.Instance.PlayBgm(BgmTypeEnum.TitleBGM);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {
                b = SoundPlayerMono.Instance.PlayBgm(BgmTypeEnum.InGameBGM);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                c = SoundPlayerMono.Instance.PlaySe(SeTypeEnum.bubbleSE);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                d = SoundPlayerMono.Instance.PlaySe(SeTypeEnum.clapSE);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                SoundPlayerMono.Instance.StopSound(a);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.H))
            {
                SoundPlayerMono.Instance.StopSound(b);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.J))
            {
                SoundPlayerMono.Instance.StopSound(c);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.K))
            {
                SoundPlayerMono.Instance.StopSound(d);
            }

        }
    }
}
