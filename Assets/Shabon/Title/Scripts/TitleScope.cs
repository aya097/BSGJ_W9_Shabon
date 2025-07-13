#nullable enable

using Shabon.Input;
using VContainer;
using VContainer.Unity;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルのスコープ
    /// </summary>
    public class TitleScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Model
            // まだない

            // View
            builder.RegisterComponentInHierarchy<TitleViewMono>();
            builder.Register<InputManager>(Lifetime.Scoped).As<IInputManager>();
            builder.Register<SerialInput>(Lifetime.Scoped);

            // Presenter
            builder.RegisterEntryPoint<TitlePresenter>(Lifetime.Scoped);
        }
    }
}