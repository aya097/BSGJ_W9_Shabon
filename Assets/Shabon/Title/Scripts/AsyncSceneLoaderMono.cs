using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Shabon.Utility;

namespace Shabon.Title
{
    public class AsyncSceneLoaderMono : MonoBehaviour
    {
        public bool CanTransitionGameScene { get; set; } = false;

        public IEnumerator Start()
        {
            AsyncOperation asyncOperation = SceneTransition.Transition(SceneName.GameScene, true);

            while (asyncOperation.progress < 0.9f || !CanTransitionGameScene)
            {
                Debug.Log(asyncOperation.progress);
                yield return null;
            }

            asyncOperation.allowSceneActivation = true;
        }

    }
}
