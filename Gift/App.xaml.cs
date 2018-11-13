using Avalonia;
using Avalonia.Markup.Xaml;

namespace Gift
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
