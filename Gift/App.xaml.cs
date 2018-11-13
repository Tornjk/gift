using Avalonia;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;
using System.Reflection;

namespace Gift
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetAssembly(typeof(App)));
        }
    }
}
