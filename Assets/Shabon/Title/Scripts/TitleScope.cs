#nullable enable

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

            // Presenter
            builder.RegisterEntryPoint<TitlePresenter>(Lifetime.Scoped);
        }
    }
}