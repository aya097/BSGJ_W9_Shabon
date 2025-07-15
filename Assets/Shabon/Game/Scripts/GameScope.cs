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
using Shabon.Menu;
using Shabon.Ui;
using Shabon.Tutorial;

namespace Shabon.Game
{
    /// <summary>
    /// インゲームのスコープ
    /// </summary>
    public class GameScope : LifetimeScope
    {
        [SerializeField] GameRuleParam gameRuleParam = null!;
        [SerializeField] BubbleParam bubbleParam = null!;
        [SerializeField] ComboViewParam comboViewParam = null!;
        [SerializeField] DirtViewParam dirtViewParam = null!;


        protected override void Configure(IContainerBuilder builder)
        {
            // Game
            builder.Register<GamePhases>(Lifetime.Scoped).As<IGamePhases>();
            builder.RegisterEntryPoint<PhaseExecutor>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();


            // Bubble
            builder.Register<BubbleSpawner>(Lifetime.Scoped).As<IBubbleSpawner>();
            builder.Register<BubbleCluster>(Lifetime.Scoped);
            builder.Register<NormalBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>().AsSelf();
            builder.Register<ArmorBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>().AsSelf();
            builder.Register<BossBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>().AsSelf();
            builder.Register<BubbleHandler>(Lifetime.Scoped).As<IBubbleHandler>();
            builder.RegisterComponentInHierarchy<BreathGetterViewMono>();
            builder.Register<BubbleMono>(Lifetime.Scoped);
            builder.Register<ArmorBubbleMono>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<WaitingAreaCheckerMono>().As<IAreaChecker>();
            builder.RegisterEntryPoint<BubbleCombo>(Lifetime.Scoped);

            // Param
            builder.RegisterInstance(gameRuleParam).AsImplementedInterfaces();
            builder.RegisterInstance(bubbleParam).AsImplementedInterfaces();
            builder.RegisterInstance(comboViewParam).AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<RuntimeObjectServer>().AsImplementedInterfaces();

            // Score
            builder.RegisterInstance(0);
            builder.Register<ScoreValue>(Lifetime.Scoped).As<IScoreValue>();
            builder.Register<DirtValue>(Lifetime.Scoped).As<IDirtValue>();

            // Breath
            builder.Register<BreathModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<BreathViewMono>();
            builder.RegisterComponentInHierarchy<WindmillViewMono>();

            // Input
            builder.Register<InputManager>(Lifetime.Scoped).As<IInputManager>();
            builder.RegisterEntryPoint<InputPresenter>(Lifetime.Scoped);
            builder.Register<SerialInput>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

            // Clap
            builder.Register<ClapModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<ClapGetterViewMono>(); // ClapGetterViewMonoを登録
            builder.RegisterComponentInHierarchy<ClapViewMono>();

            // Debug
            builder.RegisterComponentInHierarchy<DebugDirtValueViewMono>();
            builder.RegisterComponentInHierarchy<DebugComboViewMono>();
            builder.RegisterComponentInHierarchy<DebugScoreViewMono>();

            // Menu
            builder.Register<MenuModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<MenuViewMono>();
            builder.Register<MenuPresenter>(Lifetime.Scoped);
            builder.RegisterEntryPoint<MenuPresenter>(Lifetime.Scoped);

            // Ui
            builder.RegisterComponentInHierarchy<DirtValueViewMono>();
            builder.RegisterEntryPoint<InGameUiPresenter>();
            builder.Register<ComboSpawner>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<ClapUiViewMono>();
            builder.RegisterComponentInHierarchy<ClockViewMono>();
            builder.RegisterComponentInHierarchy<ResultViewMono>();
            builder.RegisterComponentInHierarchy<ScoreUiViewMono>();
            builder.RegisterComponentInHierarchy<TutorialViewMono>();

            // DirtEffect
            builder.RegisterInstance(dirtViewParam).AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<DirtEffectCollectorMono>();

            // Tutorial
            builder.RegisterEntryPoint<TutorialFacilitator>(Lifetime.Scoped).AsSelf();
            builder.Register<TutorialBubbleSpawner>(Lifetime.Scoped);

            // NormalBubbleBuilderを登録
            builder.Register<NormalBubbleBuilder>(Lifetime.Scoped).As<IBubbleBuilder>();
        }
    }
}