using ReactiveUI;

namespace Gift.Wpf.View
{
    public partial class ActiveProfileView
    {
        public ActiveProfileView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Friends, x => x.FriendListView.ViewModel));
                d(this.OneWayBind(this.ViewModel, x => x.Profile, x => x.ProfileView.ViewModel));
                d(this.OneWayBind(this.ViewModel, x => x.FriendAdd, x => x.FriendAddView.ViewModel));
                d(this.OneWayBind(this.ViewModel, x => x.Friends.SelectedFriend.Conversation, x => x.ConversationView.ViewModel));
            });
        }
    }
}
