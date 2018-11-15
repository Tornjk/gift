using ReactiveUI;

namespace Gift.Wpf.View
{
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Profiles, x => x.AvailableList.ItemsSource));
                d(this.Bind(this.ViewModel, x => x.SelectedProfile, x => x.AvailableList.SelectedItem));
                d(this.OneWayBind(this.ViewModel, x => x.SelectedProfile, x => x.ProfileViewHost.ViewModel));
                d(this.BindCommand(this.ViewModel, x => x.CreateNew, x => x.CreateNew));
            });
        }
    }
}
