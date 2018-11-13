using ReactiveUI;
using System;
using SharpTox;
using SharpTox.Core;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Gift.ViewModel
{
    public class FriendListViewModel : ReactiveObject
    {
        public FriendListViewModel(Tox tox, ICollection<int> friends)
        {
        }
    }

    public class FriendListEntryViewModel : ReactiveObject
    {
        public FriendListEntryViewModel(Tox tox, int friendNumber)
        {
            var publicKey = tox.GetFriendPublicKey(friendNumber);
            //EventHandler<ToxEventArgs.StatusEventArgs
            //Observable.FromEventPattern<ToxEventArgs.FriendConnectionStatusEventArgs>(h => tox.OnFriendConnectionStatusChanged += h, h => tox.OnFriendConnectionStatusChanged -= h)
            //tox.GetFriendStatus
        }

        [ObservableAsProperty]
        public bool IsAway { get; }
    }
}
