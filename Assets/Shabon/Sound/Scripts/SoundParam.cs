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
        [SerializeField] List<BgmInformation> bgmInfo = new();
        [SerializeField] List<SeInformation> seInfo = new();

        public IEnumerable<BgmInformation> BgmInfo => bgmInfo;
        public IEnumerable<SeInformation> SeInfo => seInfo;
    }

    [Serializable]

    public class BgmInformation
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private BgmTypeEnum bgmType;

        public AudioClip Clip => clip;
        public BgmTypeEnum BgmType => bgmType;
    }
    [Serializable]
    public class SeInformation
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private SeTypeEnum seType;

        public AudioClip Clip => clip;
        public SeTypeEnum SeType => seType;
    }

    /// <summary>
    /// 音源の種類
    /// </summary>
    public enum BgmTypeEnum
    {
        TitleBGM,
        InGameBGM,
    }
    public enum SeTypeEnum
    {
        bubbleSE,
        clapSE,
    }

}