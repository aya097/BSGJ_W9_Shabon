#nullable enable

using System.Linq;
using R3;
using Shabon.Bubble;
using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;

namespace Shabon.Tutorial
{
    /// <summary>
    /// TutorialのためのBubbleをスポーンさせる
    /// </summary>
    public class TutorialBubbleSpawner
    {
        private readonly IPlayerTransform _playerTransform;

        private readonly IBubbleParam _bubbleParam;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IAreaChecker _waitAreaChecker;
        private readonly IBubbleSpawnedArea _bubbleSpawnedArea;
        private readonly IDirtValue _dirtValue = null!;



        [Inject]
        public TutorialBubbleSpawner(
            IPlayerTransform playerTransform,
            IBubbleParam bubbleParam,
            BubbleCluster bubbleCluster,
            IAreaChecker waitAreaChecker,
            IBubbleSpawnedArea bubbleSpawnedArea,
            IDirtValue dirtValue)
        {
            _playerTransform = playerTransform;
            _bubbleParam = bubbleParam;
            _bubbleCluster = bubbleCluster;
            _waitAreaChecker = waitAreaChecker;
            _bubbleSpawnedArea = bubbleSpawnedArea;
            _dirtValue = dirtValue;
        }

        // スポーン
        public void Spawn(BubbleType bubbleType, BubbleSpawnedAreaType spawnArea)
        {
            // bubbleType別にdataを取得する
            IBubbleData bubbleData = _bubbleParam.GetBubbleDataList().Where(b => b.BubbleType == bubbleType).FirstOrDefault();


            // スポーン位置を作成
            Vector3 spawningPosition = DecideSpawningPosition(_bubbleSpawnedArea.GetArea(spawnArea));

            // バブルを生成
            var bubbleMono = GameObject.Instantiate(bubbleData.BubblePrefab, spawningPosition, Quaternion.identity);

            // Viewを取得
            var bubbleViewSelector = bubbleMono.gameObject.GetComponentInChildren<BubbleViewSelectorMono>();
            var bubbleViewMono = bubbleViewSelector.GetViewMono(bubbleType);
            // 選択されたViewのみActiveにする
            bubbleViewMono.gameObject.SetActive(true);

            Build(bubbleMono, bubbleMono, bubbleData, bubbleViewMono);

            // Clusterに追加
            _bubbleCluster.Add(bubbleMono);
        }

        // ビルド
        private void Build(
            IBubbleBuildSetter bubbleSetter,
            IBubbleMono bubbleMono,
            IBubbleData bubbleData,
            BubbleViewMono bubbleViewMono)
        {
            // BubbleMoverの生成
            IBubbleMover bubbleMover = new NormalBubbleMover(bubbleMono.Transform, bubbleData.ForwardVelocity, _playerTransform.PlayerTransform);

            // OnReach
            bubbleSetter.OnReach += () =>
           {
               bubbleMono.IsReached = true;
               // 待機時間後にdestroy
               Observable.Timer(System.TimeSpan.FromSeconds(2f))
                   .Subscribe(_ =>
                   {
                       if ((bubbleMono as MonoBehaviour) != null)
                       {
                           bubbleMono.IsAttacking = true;
                           bubbleViewMono.SetHighlight(HighLightType.Attack);
                           bubbleViewMono.PlayAttack(() =>
                            {
                                _dirtValue.Increase(4);
                                DestroyBubble(bubbleMono);
                            });
                       }
                   });
           };

            // OnBreath
            bubbleSetter.OnBreath += (arg) =>
            {

                bubbleMono.IsBreathing = true;

                // 息が吹かれた時のアニメーションを再生
                bubbleViewMono.PlayBreath(bubbleMono);
                bubbleViewMono.SetHighlight(HighLightType.Breathed);

                // Playerと逆の方向
                Vector3 moveDirection = bubbleMono.Transform.position - _playerTransform.PlayerTransform.position;
                moveDirection.y = 0;
                moveDirection = moveDirection.normalized * arg.Strength / (bubbleMono.Transform.position - arg.Position).magnitude;  // Breathの強さに比例、距離に反比例
                bubbleMover.MoveByBreath(moveDirection);

            };


            // プレゼンター処理？
            Observable.EveryValueChanged(bubbleMono, b => b.IsClapable)
                     .Subscribe(clapable =>
                     {
                         if (clapable)
                         {
                             bubbleViewMono.SetHighlight(HighLightType.Clapable);
                         }
                         else
                         {
                             bubbleViewMono.SetHighlight(HighLightType.None);
                         }
                     }).AddTo(bubbleViewMono);

            bubbleSetter.SetBuildParam(bubbleMover, _waitAreaChecker, bubbleData, _bubbleCluster);
        }

        private Vector3 DecideSpawningPosition(BoxArea boxArea)
        {
            Vector3 rand = new Vector3(Random.value, Random.value, Random.value);
            // randの範囲を-0.5~0.5に
            rand = rand - Vector3.one * 0.5f;

            // 座標 + 範囲 * (-1~1)
            return boxArea.Position + Vector3.Scale(boxArea.Size, rand);
        }

        private void DestroyBubble(IBubbleMono bubbleMono)
        {
            // Clusterから削除
            _bubbleCluster.Remove(bubbleMono);

            // Destroy
            GameObject.Destroy(bubbleMono.Transform?.gameObject);
        }
    }
}