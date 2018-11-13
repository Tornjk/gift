using ReactiveUI;
using SharpTox.Core;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Gift.ViewModel
{
    public static class ToxEvents
    {
        public static ToxEventObservables Events(this Tox tox, IScheduler scheduler = null)
        {
            if (tox == null)
            {
                throw new ArgumentNullException(nameof(tox));
            }

            if (scheduler == null)
            {
                scheduler = RxApp.MainThreadScheduler;
            }

            return new ToxEventObservables(tox, scheduler);
        }
    }

    public sealed class ToxEventObservables
    {
        private readonly Tox tox;
        private readonly IScheduler scheduler;

        public ToxEventObservables(Tox tox, IScheduler scheduler)
        {
            this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

            this.Friends = new ToxFriendObservables(tox, scheduler);
            this.Files = new ToxFileObservables(tox, scheduler);
            this.Group = new ToxGroupObservables(tox, scheduler);
        }

        public ToxFriendObservables Friends { get; }

        public ToxFileObservables Files { get; }

        public ToxGroupObservables Group { get; }

        public IObservable<ToxEventArgs.ConnectionStatusEventArgs> ConnectionStatus
            => Observable.FromEventPattern<ToxEventArgs.ConnectionStatusEventArgs>(h => tox.OnConnectionStatusChanged += h, h => tox.OnConnectionStatusChanged -= h)
                         .Select(x => x.EventArgs)
                         .ObserveOn(this.scheduler);

        public IObservable<ToxEventArgs.ReadReceiptEventArgs> ReadReceiptReceived
            => Observable.FromEventPattern<ToxEventArgs.ReadReceiptEventArgs>(h => tox.OnReadReceiptReceived += h, h => tox.OnReadReceiptReceived -= h)
                         .Select(x => x.EventArgs)
                         .ObserveOn(this.scheduler);

        public sealed class ToxGroupObservables
        {
            private readonly Tox tox;
            private readonly IScheduler scheduler;

            public ToxGroupObservables(Tox tox, IScheduler scheduler)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
                this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            }

            public IObservable<ToxEventArgs.GroupActionEventArgs> Action
                => Observable.FromEventPattern<ToxEventArgs.GroupActionEventArgs>(h => tox.OnGroupAction += h, h => tox.OnGroupAction -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.GroupMessageEventArgs> Message
                => Observable.FromEventPattern<ToxEventArgs.GroupMessageEventArgs>(h => tox.OnGroupMessage += h, h => tox.OnGroupMessage -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.GroupInviteEventArgs> Invite
                => Observable.FromEventPattern<ToxEventArgs.GroupInviteEventArgs>(h => tox.OnGroupInvite += h, h => tox.OnGroupInvite -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.GroupNamelistChangeEventArgs> NamelistChange
                => Observable.FromEventPattern<ToxEventArgs.GroupNamelistChangeEventArgs>(h => tox.OnGroupNamelistChange += h, h => tox.OnGroupNamelistChange -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.GroupTitleEventArgs> TitleChanged
                => Observable.FromEventPattern<ToxEventArgs.GroupTitleEventArgs>(h => tox.OnGroupTitleChanged += h, h => tox.OnGroupTitleChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);
        }

        public sealed class ToxFileObservables
        {
            private readonly Tox tox;
            private readonly IScheduler scheduler;

            public ToxFileObservables(Tox tox, IScheduler scheduler)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
                this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            }

            public IObservable<ToxEventArgs.FileControlEventArgs> ControlReceived
                => Observable.FromEventPattern<ToxEventArgs.FileControlEventArgs>(h => tox.OnFileControlReceived += h, h => tox.OnFileControlReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FileChunkEventArgs> ChunkReceived
                => Observable.FromEventPattern<ToxEventArgs.FileChunkEventArgs>(h => tox.OnFileChunkReceived += h, h => tox.OnFileChunkReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FileSendRequestEventArgs> SendRequestReceived
                => Observable.FromEventPattern<ToxEventArgs.FileSendRequestEventArgs>(h => tox.OnFileSendRequestReceived += h, h => tox.OnFileSendRequestReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FileRequestChunkEventArgs> ChunkRequested
                => Observable.FromEventPattern<ToxEventArgs.FileRequestChunkEventArgs>(h => tox.OnFileChunkRequested += h, h => tox.OnFileChunkRequested -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);
        }

        public sealed class ToxFriendObservables
        {
            private readonly Tox tox;
            private readonly IScheduler scheduler;

            public ToxFriendObservables(Tox tox, IScheduler scheduler)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
                this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            }

            public IObservable<ToxEventArgs.FriendConnectionStatusEventArgs> ConnectionStatus
                => Observable.FromEventPattern<ToxEventArgs.FriendConnectionStatusEventArgs>(h => tox.OnFriendConnectionStatusChanged += h, h => tox.OnFriendConnectionStatusChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FriendMessageEventArgs> Message
                => Observable.FromEventPattern<ToxEventArgs.FriendMessageEventArgs>(h => tox.OnFriendMessageReceived += h, h => tox.OnFriendMessageReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FriendRequestEventArgs> Request
                => Observable.FromEventPattern<ToxEventArgs.FriendRequestEventArgs>(h => tox.OnFriendRequestReceived += h, h => tox.OnFriendRequestReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.NameChangeEventArgs> NameChanged
                => Observable.FromEventPattern<ToxEventArgs.NameChangeEventArgs>(h => tox.OnFriendNameChanged += h, h => tox.OnFriendNameChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.StatusMessageEventArgs> StatusMessageChanged
                => Observable.FromEventPattern<ToxEventArgs.StatusMessageEventArgs>(h => tox.OnFriendStatusMessageChanged += h, h => tox.OnFriendStatusMessageChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.StatusEventArgs> StatusChanged
                => Observable.FromEventPattern<ToxEventArgs.StatusEventArgs>(h => tox.OnFriendStatusChanged += h, h => tox.OnFriendStatusChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.TypingStatusEventArgs> TypingChanged
                => Observable.FromEventPattern<ToxEventArgs.TypingStatusEventArgs>(h => tox.OnFriendTypingChanged += h, h => tox.OnFriendTypingChanged -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FriendPacketEventArgs> LossyPacketReceived
                => Observable.FromEventPattern<ToxEventArgs.FriendPacketEventArgs>(h => tox.OnFriendLossyPacketReceived += h, h => tox.OnFriendLossyPacketReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);

            public IObservable<ToxEventArgs.FriendPacketEventArgs> LosslessPacketReceived
                => Observable.FromEventPattern<ToxEventArgs.FriendPacketEventArgs>(h => tox.OnFriendLosslessPacketReceived += h, h => tox.OnFriendLosslessPacketReceived -= h)
                             .Select(x => x.EventArgs)
                             .ObserveOn(this.scheduler);
        }
    }
}
