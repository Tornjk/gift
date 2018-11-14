using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gift.Wpf.View
{
    public partial class FriendConversationView
    {
        public FriendConversationView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Messages, x => x.ConversationListBox.ItemsSource));
                d(this.Bind(this.ViewModel, x => x.InputMessage, x => x.InputMessageTextBox.Text));
                d(this.BindCommand(this.ViewModel, x => x.SendTypedMessage, x => x.SendButton));
            });
        }
    }
}
