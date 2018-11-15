using Gift.ViewModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace Gift.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            var viewmodel = new BehaviorSubject<ViewModelBase>(new LoginViewModel());

            this.Current = viewmodel.AsObservable();
            //viewmodel.ToProperty(this, x => x.Current);

            var preparing = new Subject<bool>();
            viewmodel.AsObservable()
                    .OfType<LoginViewModel>()
                    .Select(vm => vm.WhenAnyObservable(x => x.SelectedProfile.Login))
                    .Switch()
                    .Subscribe(session =>
                    {
                        foreach (var node in Nodes)
                        {
                            session.Tox.Bootstrap(node);
                        }

                        session.Tox.Start();
                        preparing.OnNext(true);
                        session.Tox.Events()
                            .ConnectionStatus
                            .Where(x => x.Status != ToxConnectionStatus.None)
                            .Take(1)
                            .Do(_ => preparing.OnNext(false))
                            .Select(_ => (ViewModelBase)new ActiveProfileViewModel(session))
                            .Subscribe(viewmodel);
                    });

            viewmodel.AsObservable()
                    .OfType<ActiveProfileViewModel>()
                    .Select(x => x.Logout)
                    .Switch()
                    .Select(_ => (ViewModelBase)new LoginViewModel())
                    .Subscribe(viewmodel);
        }

        public IObservable<ViewModelBase> Current { get; }

        [ObservableAsProperty]
        public bool Preparing { get; }

        private static readonly ToxNode[] Nodes = new ToxNode[]
        {
            new ToxNode("node.tox.biribiri.org", 33445, new ToxKey(ToxKeyType.Public, "F404ABAA1C99A9D37D61AB54898F56793E1DEF8BD46B1038B9D822E8460FAB67")),
            new ToxNode("163.172.136.118", 33445, new ToxKey(ToxKeyType.Public, "2C289F9F37C20D09DA83565588BF496FAB3764853FA38141817A72E3F18ACA0B"))
        };
    }

    public static class ApplicationDirectory
    {
        public static string ExeDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string UserDocuments => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string UserData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string ApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public static string Combine(params string[] dirs)
        {
            string d = Path.Combine(dirs);

            if (!Directory.Exists(d))
            {
                Directory.CreateDirectory(d);
            }

            return d;
        }

        public static string Combine(string parentDirectory, string subDirectory)
        {
            string d = Path.Combine(parentDirectory, subDirectory);

            if (!Directory.Exists(d))
            {
                Directory.CreateDirectory(d);
            }

            return d;
        }
    }
}
