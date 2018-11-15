using ReactiveUI;

namespace Gift.Wpf.View
{
    public partial class NewUserProfileView
    {
        public NewUserProfileView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(this.ViewModel, x => x.Name, x => x.NameTextBox.Text));
                d(this.Bind(this.ViewModel, x => x.Password, x => x.PasswordTextBox.Text));
                d(this.BindCommand(this.ViewModel, x => x.Login, x => x.CreateButton));
            });
        }
    }
}
