
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

        public static void Transition(SceneName sceneName)
        {
            // SceneNameが登録されていなければ
            if (!_sceneDictionary.ContainsKey(sceneName))
            {
                Debug.LogWarning($"{sceneName}は登録されていないシーンです。");
                return;
            }
            SceneManager.LoadScene(_sceneDictionary[sceneName]);
        }
    }
}