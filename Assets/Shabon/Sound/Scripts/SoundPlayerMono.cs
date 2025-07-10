#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shabon.Sound
{
    /// <summary>
    /// サウンドを管理できるトークン
    /// </summary>
    public class SoundToken
    {
        public int Id { get; private set; }
        public bool IsValid { get; private set; }   // 有効なトークンか
        public SoundToken(int id, bool isValid)
        {
            Id = id;
            IsValid = isValid;
        }
    }
    /// <summary>
    /// ゲームのサウンドを再生するクラス、シングルトン
    /// </summary>
    public class SoundPlayerMono : MonoBehaviour
    {
        [SerializeField] private SoundParam soundParam = null!;
        public static SoundPlayerMono? Instance;    // シングルトン

        [SerializeField] List<AudioSource> bgmSources = new();    // BGMに使用するAudioSource
        [SerializeField] List<AudioSource> seSources = new();    // SEに使用するAudioSource

        // AudioSourceとトークンを紐づけるDictionary
        private Dictionary<AudioSource, SoundToken> _sourceTokens = new();

        // トークンを生成する数値
        private int _tokenId = 0;


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

            // Bgmはループ処理
            foreach (var source in bgmSources)
            {
                source.playOnAwake = false;
                source.loop = true;
            }

            // Se
            foreach (var source in seSources)
            {
                source.playOnAwake = false;
                source.loop = false;
            }
        }

        // Bgm再生
        public SoundToken PlayBgm(BgmTypeEnum bgmType)
        {            // Bgmが存在するか確認
            var bgmInfo = soundParam.BgmInfo.Where(i => i.BgmType == bgmType).FirstOrDefault();
            if (bgmInfo == null)
            {
                return new SoundToken(-1, false);   // 無効
            }
            // 再生可能なAudioSource
            var source = bgmSources.Where(s => !s.isPlaying).FirstOrDefault();
            if (source == null)
            {
                return new SoundToken(-1, false);   // 無効
            }
            // 再生可能な場合
            source.clip = bgmInfo.Clip;
            source.Play();

            var token = IssueToken();   // トークン生成
            _sourceTokens[source] = token; // AudioSourceと紐付け

            return token;
        }
        // Se再生
        public SoundToken PlaySe(SeTypeEnum seType, bool isLoop = false)
        {
            Debug.Log(seSources.Where(s => s.isPlaying).Count());

            // Seが存在するか確認
            var seInfo = soundParam.SeInfo.Where(i => i.SeType == seType).FirstOrDefault();
            if (seInfo == null)
            {
                return new SoundToken(-1, false);   // 無効
            }
            // 再生可能なAudioSource
            var source = seSources.Where(s => !s.isPlaying).FirstOrDefault();
            if (source == null)
            {
                return new SoundToken(-1, false);   // 無効
            }

            // 再生可能な場合
            source.clip = seInfo.Clip;
            source.loop = isLoop;
            source.Play();

            var token = IssueToken();   // トークン生成
            _sourceTokens[source] = token; // AudioSourceと紐付け

            return token;
        }

        // 再生停止
        public void StopSound(SoundToken soundToken)
        {
            // 無効ならば
            if (!soundToken.IsValid)
            {
                return;
            }
            // idを見つける
            var source = _sourceTokens.Where(pair => pair.Value == soundToken).FirstOrDefault().Key;
            if (source == null)
            {
                return;
            }

            source.Stop();
        }


        // トークンを発行
        private SoundToken IssueToken()
        {
            var token = new SoundToken(_tokenId, true);
            _tokenId++;
            return token;
        }
    }
}