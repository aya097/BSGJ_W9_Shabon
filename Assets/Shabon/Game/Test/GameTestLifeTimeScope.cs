#nullable enable
using Shabon.Param;
using Unity.VisualScripting;
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
            builder.RegisterEntryPoint<GameExecutor>(Lifetime.Scoped);


            // Param
            builder.RegisterInstance(GameRuleParam);
            builder.Register<GameRuleParamServer>(Lifetime.Scoped).AsImplementedInterfaces();

        }
    }
}