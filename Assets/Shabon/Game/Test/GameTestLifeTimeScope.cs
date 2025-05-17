#nullable enable
using Shabon.Param;
using VContainer;
using VContainer.Unity;

namespace Shabon.Game
{
    public class GameTestLifeTimeScope : LifetimeScope
    {
        public GameRuleParam GameRuleParam;
        protected override void Configure(IContainerBuilder builder)
        {
            // Game
            builder.Register<GamePhases>(Lifetime.Scoped).As<IGamePhases>();


            // Param
            builder.RegisterInstance(GameRuleParam);
            builder.Register<GameRuleParamServer>(Lifetime.Scoped).AsImplementedInterfaces();

        }
    }
}