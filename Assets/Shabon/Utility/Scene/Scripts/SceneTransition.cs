
#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shabon.Utility
{
    /// <summary>
    /// 使用するシーンをまとめたEnum
    /// </summary>
    public enum SceneName
    {
        GameScene,
        TitleScene,
        ResultScene,
    }
    /// <summary>
    /// シーンの遷移を行うクラス
    /// </summary>
    public static class SceneTransition
    {
        // SceneNameとstring変換用
        private static readonly Dictionary<SceneName, string> _sceneDictionary = new()
        {
            {SceneName.GameScene, "GameScene"},
            {SceneName.TitleScene, "TitleScene"},
            {SceneName.ResultScene, "ResultScene"},
        };

        public static AsyncOperation Transition(SceneName sceneName, bool isAsync = false)
        {
            // SceneNameが登録されていなければ
            if (!_sceneDictionary.ContainsKey(sceneName))
            {
                Debug.LogWarning($"{sceneName}は登録されていないシーンです。");
                return null!;
            }

            // 非同期処理でのロードのみ戻り値返す
            if (isAsync)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneDictionary[sceneName]);
                asyncOperation.allowSceneActivation = false;
                return asyncOperation;
            }
            else
            {
                SceneManager.LoadScene(_sceneDictionary[sceneName]);
                return null!;
            }
        }
    }
}