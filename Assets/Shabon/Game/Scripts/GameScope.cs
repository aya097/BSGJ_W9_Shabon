#nullable enable
using Shabon.Bubble;
using Shabon.Param;
using Shabon.Score;
using Shabon.Breath;
using Shabon.Input;
using Shabon.Clap;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Shabon.SelfDebug;

namespace Shabon.Game
{
    /// <summary>
    /// インゲームのスコープ
    /// </summary>
    public class GameScope : LifetimeScope
    {
        [SerializeField] GameRuleParam gameRuleParam = null!;
        [SerializeField] BubbleParam bubbleParam = null!;
        protected override void Configure(IContainerBuilder builder)
        {
            // Game
            builder.Register<GamePhases>(Lifetime.Scoped).As<IGamePhases>();
            builder.RegisterEntryPoint<PhaseExecutor>(Lifetime.Scoped);

            // Bubble
            builder.Register<BubbleSpawner>(Lifetime.Scoped).As<IBubbleSpawner>();
            builder.Register<BubbleCluster>(Lifetime.Scoped);
            builder.Register<NormalBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>().AsSelf();
            builder.Register<BossBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>().AsSelf();
            builder.Register<BubbleHandler>(Lifetime.Scoped).As<IBubbleHandler>();
            builder.RegisterComponentInHierarchy<BreathGetterViewMono>();
            builder.Register<BubbleMono>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<WaitingAreaCheckerMono>().As<IAreaChecker>();
            builder.Register<BubbleChain>(Lifetime.Scoped).As<IBubbleChain>();
            builder.Register<BubbleCombo>(Lifetime.Scoped).As<IBubbleCombo>();

            // Param
            builder.RegisterInstance(gameRuleParam).AsImplementedInterfaces();
            builder.RegisterInstance(bubbleParam).AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<RuntimeObjectServer>().AsImplementedInterfaces();

            // Score
            builder.RegisterInstance(0);
            builder.Register<ScoreValue>(Lifetime.Scoped).As<IScoreValue>();
            builder.Register<DirtValue>(Lifetime.Scoped).As<IDirtValue>();

            // Breath
            builder.Register<BreathModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<BreathViewMono>();

            // Input
            builder.Register<InputManager>(Lifetime.Scoped).As<IInputManager>();
            builder.RegisterEntryPoint<InputPresenter>(Lifetime.Scoped);

            // Clap
            builder.Register<ClapModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<ClapGetterViewMono>(); // ClapGetterViewMonoを登録

            // Debug
            builder.RegisterComponentInHierarchy<DebugDirtValueViewMono>();
        }
    }
}