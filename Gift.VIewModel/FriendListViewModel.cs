using ReactiveUI;
using System;
using SharpTox;
using SharpTox.Core;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Linq;

namespace Gift.ViewModel
{
    public class FriendListViewModel : ReactiveObject
    {
        public FriendListViewModel(Tox tox, ICollection<int> friends)
        {
            this.Friends = friends.Select(x => new FriendListEntryViewModel(tox, x)).ToArray();
        }

        public ICollection<FriendListEntryViewModel> Friends { get; }
    }

    public class FriendListEntryViewModel : ReactiveObject
    {
        public FriendListEntryViewModel(Tox tox, int friendNumber)
        {
            //var publicKey = tox.GetFriendPublicKey(friendNumber);
            var status = tox.Events().Friends.StatusChanged
                .Where(x => x.FriendNumber == friendNumber)
                .Select(x => x.Status);

            status.Select(x => x == ToxUserStatus.Away)
                .ToPropertyEx(this, x => x.IsAway);

            status.Select(x => x == ToxUserStatus.Busy)
                .ToPropertyEx(this, x => x.IsBusy);

            tox.Events().Friends.NameChanged
                .Where(x => x.FriendNumber == friendNumber)
                .Select(x => x.Name)
                .StartWith(tox.GetFriendName(friendNumber))
                .ToPropertyEx(this, x => x.Name);

            tox.Events().Friends.StatusMessageChanged
                .Where(x => x.FriendNumber == friendNumber)
                .Select(x => x.StatusMessage)
                .StartWith(tox.GetFriendStatusMessage(friendNumber))
                .ToPropertyEx(this, x => x.StatusMessage);
        }

        [ObservableAsProperty]
        public string Name { get; }

        [ObservableAsProperty]
        public bool IsAway { get; }

        [ObservableAsProperty]
        public bool IsBusy { get; }

        [ObservableAsProperty]
        public string StatusMessage { get; }
    }
}
