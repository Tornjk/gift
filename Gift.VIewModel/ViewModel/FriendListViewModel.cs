using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Gift.ViewModel
{
    public class FriendListViewModel : ViewModelBase
    {
        public FriendListViewModel(Tox tox)
        {
            var friends = new SourceList<int>();

            foreach(var friend in tox.Friends)
            {
                friends.Add(friend);
                tox.AddFriendNoRequest(tox.GetFriendPublicKey(friend));
            }

            this.Add = ReactiveCommand.Create<int>(friendNumber =>
            {
                friends.Add(friendNumber);
            });

            ReadOnlyObservableCollection<FriendListEntryViewModel> entries;
            friends.Connect()
                .Transform(number => new FriendListEntryViewModel(tox, number))
                .Bind(out entries)
                .Subscribe();

            this.Friends = entries;

            this.Friends.ToObservableChangeSet()
                .MergeMany(x => x.Remove)
                .Subscribe(number =>
                {
                    if (tox.DeleteFriend(number))
                    {
                        friends.Remove(number);
                    }
                });
        }

        [Reactive]
        public string Filter { get; set; }

        [Reactive]
        public FriendListEntryViewModel SelectedFriend { get; set; }

        public ReactiveCommand<int, Unit> Add { get; }

        public ReadOnlyObservableCollection<FriendListEntryViewModel> Friends { get; }
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

            this.Remove = ReactiveCommand.Create(() => friendNumber);
            this.Conversation = new FriendConversationViewModel(tox, friendNumber);
        }

        [ObservableAsProperty]
        public string Name { get; }

        [ObservableAsProperty]
        public bool IsAway { get; }

        [ObservableAsProperty]
        public bool IsBusy { get; }

        [ObservableAsProperty]
        public string StatusMessage { get; }

        public ReactiveCommand<Unit, int> Remove { get; }

        public FriendConversationViewModel Conversation { get; }
    }
}
