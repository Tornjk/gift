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
    public partial class FriendListEntryView
    {
        public FriendListEntryView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Name, x => x.NameTextBlock.Text));
                d(this.OneWayBind(this.ViewModel, x => x.StatusMessage, x => x.StatusMessageTextBlock.Text));
            });
        }
    }
}
