using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Gift.Wpf.View
{
    public partial class ProfileView
    {
        public ProfileView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(this.ViewModel, x => x.Name, x => x.NameTextBox.Text));
                d(this.Bind(this.ViewModel, x => x.StatusMessage, x => x.StausTextBox.Text));
            });
        }
    }
}
