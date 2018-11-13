using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gift.Wpf
{
    static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                var app = new App();
                Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetAssembly(typeof(App)));

                var vm = new ViewModel.MainWindowViewModel();
                app.Run(new MainWindow { ViewModel = vm });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }

            return 0;
        }
    }
}
