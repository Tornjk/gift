using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gift.ViewModel;
using ReactiveUI;

namespace Gift.View
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private FriendListView FriendListView;

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif
            this.FriendListView = this.FindControl<FriendListView>("FriendListView");

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Friends, x => x.FriendListView.ViewModel));
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
