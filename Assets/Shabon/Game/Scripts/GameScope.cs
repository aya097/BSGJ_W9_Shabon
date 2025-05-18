#nullable enable
using Shabon.Bubble;
using Shabon.Param;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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

            // Param
            builder.RegisterInstance(gameRuleParam).AsImplementedInterfaces();
            builder.RegisterInstance(bubbleParam).AsImplementedInterfaces();
        }
    }
}