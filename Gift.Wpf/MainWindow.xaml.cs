using Gift.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Gift.Wpf
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Friends, x => x.FriendListView.ViewModel));
                d(this.OneWayBind(this.ViewModel, x => x.Profile, x => x.ProfileView.ViewModel));
                d(this.OneWayBind(this.ViewModel, x => x.FriendAdd, x => x.FriendAddView.ViewModel));
            });
        }
    }
}
