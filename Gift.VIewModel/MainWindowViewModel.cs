using Gift.ViewModel;
using ReactiveUI;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
                    var key = t.GetPrivateKey();
                    File.WriteAllBytes(profilePath, key.GetBytes());
                }
            }

            var data = File.ReadAllBytes(profilePath);
            var tox = new Tox(ToxOptions.Default, new ToxKey(ToxKeyType.Secret, data));

            foreach(var node in Nodes)
            {
                tox.Bootstrap(node);
            }

            tox.Start();

            while (!tox.IsConnected)
            {
                Thread.Sleep(40);
            }

            const string TornJKToxID = "737AD30A0BA6D0172E025BD05EDB27EEEE0913F207867B7A43C438BDAD128C151CE865D7D560";
            var friendid = tox.AddFriend(new ToxId(TornJKToxID), "Hello from Gift!");

            this.Friends = new FriendListViewModel(tox, new[] { friendid });
        }

        public FriendListViewModel Friends { get; }

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
