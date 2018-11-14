using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gift.Wpf.View
{
    public partial class MessageView
    {
        public MessageView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                // todo
                d(this.OneWayBind(this.ViewModel, x => x.From, x => x.HorizontalAlignment, from => from == "Me" ? HorizontalAlignment.Right : HorizontalAlignment.Left));
                d(this.OneWayBind(this.ViewModel, x => x.From, x => x.FromTextBlock.Text));

                d(this.OneWayBind(this.ViewModel, x => x.Message, x => x.MessageTextBlock.Text));
            });
        }
    }
}
