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
            builder.Register<TitleModel>(Lifetime.Scoped);
            //builder.RegisterComponentInHierarchy<AsyncSceneLoaderMono>();

            // View
            builder.RegisterComponentInHierarchy<TitleViewMono>();
            builder.Register<InputManager>(Lifetime.Scoped).As<IInputManager>();
            builder.Register<SerialInput>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<SelectLanguageViewMono>();

            // Presenter
            builder.RegisterEntryPoint<TitlePresenter>(Lifetime.Scoped);
        }
    }
}