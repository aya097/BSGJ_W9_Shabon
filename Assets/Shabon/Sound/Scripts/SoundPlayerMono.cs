#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Shabon.Sound
{
    /// <summary>
    /// ゲームのサウンドを再生するクラス、シングルトン
    /// </summary>
    public class SoundPlayerMono : MonoBehaviour
    {
        public static SoundPlayerMono? Instance;    // シングルトン

        [SerializeField] List<AudioSource> audioSources = new();    // 使用するAudioSource

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }


    }
}