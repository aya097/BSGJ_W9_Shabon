using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shabon.Sound
{
    /// <summary>
    /// AudioClipを設定するクラス
    /// </summary>
    [CreateAssetMenu(fileName = "SoundParam", menuName = "Scriptable Objects/SoundParam")]
    public class SoundParam : ScriptableObject
    {
        [SerializeField] List<SoundInformation> soundInfo = new();

        public IEnumerable<SoundInformation> SoundInfo => soundInfo;
    }

    [Serializable]
    public class SoundInformation
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private SoundTypeEnum soundType;

        public AudioClip Clip => clip;
        public SoundTypeEnum SoundType => soundType;
    }

    /// <summary>
    /// 音源の種類
    /// </summary>
    public enum SoundTypeEnum
    {
        TitleBGM,
        InGameBGM,
        SimpleSE,
        BubbleSE,

    }

}