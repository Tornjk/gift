using Gift.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Gift.Wpf.View
{
    public partial class FriendListView
    {
        public FriendListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Friends, x => x.FriendListBox.ItemsSource));
                d(this.Bind(this.ViewModel, x => x.SelectedFriend, x => x.FriendListBox.SelectedItem));
            });
        }
    }
}
