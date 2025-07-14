using System;
using UnityEngine;

namespace Shabon.Game
{
    public class RankingSceneSwiper : MonoBehaviour
    {
        [SerializeField] GameObject[] rankingPanels = null!;
        [SerializeField] float switchInterval = 3f;
        [SerializeField] float slideDistance = 1600f;
        [SerializeField] float slideDuration = 0.5f;

        private int currentIndex = 0;
        private float timer = 0f;
        private Vector2 touchStart;

        // アニメーション用
        private bool isSliding = false;
        private float slideTime = 0f;
        private int slideFrom = 0;
        private int slideTo = 0;
        private int slideDir = 1; // 1:右, -1:左

        // 交互制御用
        private bool nextRight = true;

        // ★ 追加: 初期位置を保存
        private Vector2[] originalPositions;

        void Start()
        {
            // ★ 初期位置を保存
            originalPositions = new Vector2[rankingPanels.Length];
            for (int i = 0; i < rankingPanels.Length; i++)
            {
                var rect = rankingPanels[i].GetComponent<RectTransform>();
                originalPositions[i] = rect.anchoredPosition;
            }

            for (int i = 0; i < rankingPanels.Length; i++)
            {
                rankingPanels[i].SetActive(i == 0);
                rankingPanels[i].transform.localPosition = Vector3.zero;
            }
        }

        void Update()
        {
            if (!isSliding)
            {
                timer += Time.deltaTime;
                if (timer > switchInterval)
                {
                    timer = 0f;
                    bool toRight = nextRight;
                    nextRight = !nextRight;
                    SlideToPanel((currentIndex + 1) % rankingPanels.Length, toRight);
                }

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
                if (UnityEngine.Input.touchCount == 1)
                {
                    var touch = UnityEngine.Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                        touchStart = touch.position;
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        float dx = touch.position.x - touchStart.x;
                        if (Mathf.Abs(dx) > 100)
                        {
                            bool toRight = dx < 0;
                            nextRight = !toRight;
                            SlideToPanel((currentIndex + (toRight ? 1 : -1) + rankingPanels.Length) % rankingPanels.Length, toRight);
                            timer = 0f;
                        }
                    }
                }
                if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                {
                    nextRight = false;
                    SlideToPanel((currentIndex + 1) % rankingPanels.Length, true);
                    timer = 0f;
                }
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    nextRight = true;
                    SlideToPanel((currentIndex - 1 + rankingPanels.Length) % rankingPanels.Length, false);
                    timer = 0f;
                }
#endif
            }
            else
            {
                slideTime += Time.deltaTime;
                float t = Mathf.Clamp01(slideTime / slideDuration);

                float fromX = 0;
                float toX = -slideDir * slideDistance;
                float nextFromX = slideDir * slideDistance;
                float nextToX = 0;

                rankingPanels[slideFrom].transform.localPosition = new Vector3(Mathf.Lerp(fromX, toX, t), 0, 0);
                rankingPanels[slideTo].transform.localPosition = new Vector3(Mathf.Lerp(nextFromX, nextToX, t), 0, 0);

                if (t >= 1f)
                {
                    rankingPanels[slideFrom].SetActive(false);
                    rankingPanels[slideTo].transform.localPosition = Vector3.zero;

                    currentIndex = slideTo;
                    isSliding = false;

                    // スライド終了時のみ位置リセット
                    ResetPanelPositions();
                }
            }

            // ResetPanelPositions(); ←この行を削除
        }

        // ★ 変更: スライド中は2枚以外のみリセット
        private void ResetPanelPositions()
        {
            for (int i = 0; i < rankingPanels.Length; i++)
            {
                if (isSliding && (i == slideFrom || i == slideTo))
                {
                    // スライド中の2枚はアニメーションに任せる
                    continue;
                }
                rankingPanels[i].transform.localPosition = Vector3.zero;
                // anchoredPositionは触らない
            }
        }


        private void SlideToPanel(int nextIndex, bool directionRight)
        {
            if (nextIndex == currentIndex || isSliding) return;

            slideFrom = currentIndex;
            slideTo = nextIndex;
            slideDir = directionRight ? 1 : -1;
            slideTime = 0f;
            isSliding = true;

            rankingPanels[slideTo].SetActive(true);
            rankingPanels[slideTo].transform.localPosition = new Vector3(slideDir * slideDistance, 0, 0);
        }
    }
}