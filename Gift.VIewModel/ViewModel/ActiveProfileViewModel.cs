using ReactiveUI;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Gift.ViewModel
{
    public class ActiveProfileViewModel : ViewModelBase
    {
        // receives a RUNNING toxSession for which now we are here responsible
        public ActiveProfileViewModel(ToxSession toxSession)
        {
            this.Friends = new FriendListViewModel(toxSession.Tox);
            this.FriendAdd = new FriendAddViewModel(toxSession.Tox);
            this.Profile = new ProfileViewModel(toxSession.Tox);

            this.WhenAnyObservable(x => x.FriendAdd.Add)
                .InvokeCommand(this, x => x.Friends.Add);

            this.WhenAnyObservable(x => x.Profile.Changed)
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(_ => toxSession.Tox.GetData().Save(toxSession.ProfilePath));

            this.Logout = ReactiveCommand.Create(() =>
            {
                toxSession.Tox.Stop();
                return Unit.Default;
            });
        }

        public FriendListViewModel Friends { get; }

        public FriendAddViewModel FriendAdd { get; }

        public ProfileViewModel Profile { get; }

        public ReactiveCommand<Unit, Unit> Logout { get; }
    }
}
