using System.Windows;
using ReactiveUI;

namespace Gift.Wpf.View
{
    public partial class ExistingUserProfileView
    {
        public ExistingUserProfileView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Name, x => x.NameTextBlock.Text));
                d(this.OneWayBind(this.ViewModel, x => x.Encrypted, x => x.PasswordTextBox.Visibility,
                                    (bool encrypted) => encrypted ? Visibility.Visible : Visibility.Collapsed));
                
                d(this.Bind(this.ViewModel, x => x.Password, x => x.PasswordTextBox.Text));
                d(this.BindCommand(this.ViewModel, x => x.Login, x => x.LoginButton));
            });
        }
    }
}
