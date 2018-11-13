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
    public partial class FriendAddView
    {
        public FriendAddView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(this.ViewModel, x => x.ID, x => x.IdTextBox.Text));
                d(this.BindCommand(this.ViewModel, x => x.Add, x => x.AddButton));
            });
        }
    }
}
