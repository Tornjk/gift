using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gift.ViewModel;
using ReactiveUI;

namespace Gift.View
{
    public class FriendListView : ReactiveUserControl<FriendListViewModel>
    {
        private ListBox FriendListBox;

        public FriendListView()
        {
            this.InitializeComponent();

            this.FriendListBox = this.FindControl<ListBox>("FriendsListBox");

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Friends, x => x.FriendListBox.Items));
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
