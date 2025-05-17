#nullable enable
using Shabon.Param;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shabon.Game
{
    public class GameTestLifeTimeScope : LifetimeScope
    {
        [SerializeField] GameRuleParam GameRuleParam = null!;
        protected override void Configure(IContainerBuilder builder)
        {
            // Game
            builder.Register<GamePhases>(Lifetime.Scoped).As<IGamePhases>();
            builder.RegisterEntryPoint<PhaseExecutor>(Lifetime.Scoped);


            // Param
            builder.RegisterInstance(GameRuleParam).AsImplementedInterfaces();
        }
    }
}