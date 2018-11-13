using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gift.ViewModel;
using ReactiveUI;

namespace Gift.View
{
    public class FriendListEntryView : ReactiveUserControl<FriendListEntryViewModel>
    {
        private TextBlock StatusMessageBlock;
        public FriendListEntryView()
        {
            this.InitializeComponent();

            this.StatusMessageBlock = this.Find<TextBlock>("StatusMessageTextBlock");

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.StatusMessage, x => x.StatusMessageBlock.Text));
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
