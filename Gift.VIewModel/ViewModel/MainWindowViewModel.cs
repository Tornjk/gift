using Gift.ViewModel;
using ReactiveUI;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace Gift.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            const string Profile = "devprofile.tox";
            var profilePath = Path.Combine(ApplicationDirectory.UserData, Profile);
            if (!File.Exists(profilePath))
            {
                using (var t = new Tox(ToxOptions.Default))
                {
                    t.GetData().Save(profilePath);
                }
            }

            var data = ToxData.FromDisk(profilePath);
            var tox = new Tox(new ToxOptions(true, true), data);

            foreach(var node in Nodes)
            {
                tox.Bootstrap(node);
            }

            tox.Start();

            while (!tox.IsConnected)
            {
                Thread.Sleep(40);
            }

            this.Friends = new FriendListViewModel(tox);
            this.FriendAdd = new FriendAddViewModel(tox);
            this.Profile = new ProfileViewModel(tox);

            this.WhenAnyObservable(x => x.FriendAdd.Add)
                .InvokeCommand(this, x => x.Friends.Add);

            this.WhenAnyObservable(x => x.Profile.Changed)
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(_ => tox.GetData().Save(profilePath));
        }

        public FriendListViewModel Friends { get; }

        public FriendAddViewModel FriendAdd { get; }

        public ProfileViewModel Profile { get; }

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
