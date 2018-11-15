﻿using Gift.ViewModel;
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
                //d(this.OneWayBind(this.ViewModel, x => x.Current, x => x.ViewModelHost.ViewModel));
                d(this.WhenAnyObservable(x => x.ViewModel.Current).BindTo(this, x => x.ViewModelHost.ViewModel));
            });
        }
    }
}
