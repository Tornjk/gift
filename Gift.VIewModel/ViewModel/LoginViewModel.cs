using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using SharpTox.Encryption;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Gift.ViewModel
{
    public sealed class LoginViewModel : ViewModelBase
    {
        const string UserDataSubdirectory = "gift";

        public LoginViewModel()
        {
            var profiles = new SourceList<IProfileViewModel>();

            this.WhenAnyValue(x => x.UserProfilesDirectory)
                .Where(x => Directory.Exists(x))
                .Select(x => Directory.EnumerateFiles(x, "*.tox").AsObservableChangeSet())
                .Switch()
                .Transform(x => (IProfileViewModel)new ExistingUserProfileViewModel(x, ToxData.FromDisk(x)))
                .PopulateInto(profiles);

            ReadOnlyObservableCollection<IProfileViewModel> all;
            profiles.Connect()
                .Bind(out all)
                .Subscribe();

            this.Profiles = all;

            this.CreateNew = ReactiveCommand.Create<IProfileViewModel>(() => new NewUserProfileViewModel(this.UserProfilesDirectory),
                                                                       this.WhenAnyValue(x => x.UserProfilesDirectory, (string s) => Directory.Exists(s)));

            this.WhenAnyObservable(x => x.CreateNew)
                .BindTo(this, x => x.SelectedProfile);
        }

        [Reactive]
        public string UserProfilesDirectory { get; set; } = ApplicationDirectory.Combine(ApplicationDirectory.UserData, UserDataSubdirectory);

        [Reactive]
        public IProfileViewModel SelectedProfile { get; set; }

        public ReadOnlyObservableCollection<IProfileViewModel> Profiles { get; }

        public ReactiveCommand<Unit, IProfileViewModel> CreateNew { get; }
    }

    public sealed class ToxSession : IDisposable
    {
        public ToxSession(Tox tox, string profilePath)
        {
            this.Tox = tox ?? throw new ArgumentNullException(nameof(tox));
            this.ProfilePath = profilePath ?? throw new ArgumentNullException(nameof(profilePath));
        }

        public Tox Tox { get; }

        public string ProfilePath { get; }

        void IDisposable.Dispose() => this.Tox.Dispose();
    }

    public interface IProfileViewModel
    {
        bool Encrypted { get; }

        string Name { get; }

        ReactiveCommand<Unit, ToxSession> Login { get; }
    }

    public sealed class ExistingUserProfileViewModel : ReactiveObject, IProfileViewModel
    {
        public string FilePath { get; }

        public ToxData Data { get; }

        public ReactiveCommand<Unit, ToxSession> Login { get; }

        [Reactive]
        public string Password { get; set; }

        public string Name => Path.GetFileNameWithoutExtension(this.FilePath);

        public bool Encrypted => this.Data.IsEncrypted;

        public ExistingUserProfileViewModel(string filePath, ToxData data)
        {
            this.FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.Data = data ?? throw new ArgumentNullException(nameof(data));

            this.Login = ReactiveCommand.Create(() =>
            {
                Tox tox = null;
                if (this.Encrypted)
                {
                    tox = new Tox(ToxOptions.Default(), this.Data, this.Password);
                }
                else
                {
                    tox = new Tox(ToxOptions.Default(), this.Data);
                }

                return new ToxSession(tox, filePath);
            });
        }
    }

    public sealed class NewUserProfileViewModel : ReactiveObject, IProfileViewModel
    {
        public NewUserProfileViewModel(string directory)
        {
            this.WhenAnyValue(x => x.Password)
                .Select(x => !string.IsNullOrEmpty(x))
                .ToPropertyEx(this, x => x.Encrypted);

            this.Login = ReactiveCommand.Create(() =>
            {
                var filePath = Path.Combine(directory, this.Name + ".tox");
                var tox = new Tox(ToxOptions.Default());
                if (this.Encrypted)
                {
                    var data = tox.GetData().Bytes;
                    ToxEncryption.Encrypt(data, this.Password, out _);
                    var toxdata = ToxData.FromBytes(data);
                    tox.Dispose();
                    toxdata.Save(filePath);

                    tox = new Tox(ToxOptions.Default(), toxdata, this.Password);
                }

                return new ToxSession(tox, filePath);
            }, this.WhenAnyValue(x => x.Name)
                   .Select(fileName =>
                   {
                       if (string.IsNullOrWhiteSpace(fileName))
                       {
                           return false;
                       }

                       var filePath = Path.Combine(directory, fileName + ".tox");
                       if (File.Exists(filePath))
                       {
                           return false;
                       }

                       if ((from i in Path.GetInvalidFileNameChars()
                            from c in fileName
                            select i == c).Any(x => x))
                       {
                           return false;
                       }

                       return true;
                   }));
        }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Password { get; set; }

        [ObservableAsProperty]
        public bool Encrypted { get; }

        public ReactiveCommand<Unit, ToxSession> Login { get; }
    }
}
