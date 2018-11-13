using System;
using System.Reactive.Linq;
using SharpTox.Core;

namespace Gift.ViewModel
{
    public static class ToxEvents
    {
        public static ToxEventObservables Events(this Tox tox) => new ToxEventObservables(tox ?? throw new ArgumentNullException(nameof(tox)));
    }

    public sealed class ToxEventObservables
    {
        private readonly Tox tox;

        public ToxEventObservables(Tox tox)
        {
            this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
            this.Friends = new ToxFriendObservables(tox);
            this.Files = new ToxFileObservables(tox);
            this.Group = new ToxGroupObservables(tox);
        }

        public ToxFriendObservables Friends { get; }

        public ToxFileObservables Files { get; }

        public ToxGroupObservables Group { get; }

        public IObservable<ToxEventArgs.ConnectionStatusEventArgs> ConnectionStatus
            => Observable.FromEventPattern<ToxEventArgs.ConnectionStatusEventArgs>(h => tox.OnConnectionStatusChanged += h, h => tox.OnConnectionStatusChanged -= h)
                         .Select(x => x.EventArgs);

        public IObservable<ToxEventArgs.ReadReceiptEventArgs> ReadReceiptReceived
            => Observable.FromEventPattern<ToxEventArgs.ReadReceiptEventArgs>(h => tox.OnReadReceiptReceived += h, h => tox.OnReadReceiptReceived -= h)
                         .Select(x => x.EventArgs);

        public sealed class ToxGroupObservables
        {
            private readonly Tox tox;

            public ToxGroupObservables(Tox tox)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
            }

            public IObservable<ToxEventArgs.GroupActionEventArgs> Action
                => Observable.FromEventPattern<ToxEventArgs.GroupActionEventArgs>(h => tox.OnGroupAction += h, h => tox.OnGroupAction -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.GroupMessageEventArgs> Message
                => Observable.FromEventPattern<ToxEventArgs.GroupMessageEventArgs>(h => tox.OnGroupMessage += h, h => tox.OnGroupMessage -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.GroupInviteEventArgs> Invite
                => Observable.FromEventPattern<ToxEventArgs.GroupInviteEventArgs>(h => tox.OnGroupInvite += h, h => tox.OnGroupInvite -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.GroupNamelistChangeEventArgs> NamelistChange
                => Observable.FromEventPattern<ToxEventArgs.GroupNamelistChangeEventArgs>(h => tox.OnGroupNamelistChange += h, h => tox.OnGroupNamelistChange -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.GroupTitleEventArgs> TitleChanged
                => Observable.FromEventPattern<ToxEventArgs.GroupTitleEventArgs>(h => tox.OnGroupTitleChanged += h, tox.OnGroupTitleChanged -= h)
                             .Select(x => x.EventArgs);
        }

        public sealed class ToxFileObservables
        {
            private readonly Tox tox;

            public ToxFileObservables(Tox tox)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
            }

            public IObservable<ToxEventArgs.FileControlEventArgs> ControlReceived
                => Observable.FromEventPattern<ToxEventArgs.FileControlEventArgs>(h => tox.OnFileControlReceived += h, h => tox.OnFileControlReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FileChunkEventArgs> ChunkReceived
                => Observable.FromEventPattern<ToxEventArgs.FileChunkEventArgs>(h => tox.OnFileChunkReceived += h, h => tox.OnFileChunkReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FileSendRequestEventArgs> SendRequestReceived
                => Observable.FromEventPattern<ToxEventArgs.FileSendRequestEventArgs>(h => tox.OnFileSendRequestReceived += h, h => tox.OnFileSendRequestReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FileRequestChunkEventArgs> ChunkRequested
                => Observable.FromEventPattern<ToxEventArgs.FileRequestChunkEventArgs>(h => tox.OnFileChunkRequested += h, h => tox.OnFileChunkRequested -= h)
                             .Select(x => x.EventArgs);
        }

        public sealed class ToxFriendObservables
        {
            private readonly Tox tox;

            public ToxFriendObservables(Tox tox)
            {
                this.tox = tox ?? throw new ArgumentNullException(nameof(tox));
            }

            public IObservable<ToxEventArgs.FriendConnectionStatusEventArgs> ConnectionStatus
                => Observable.FromEventPattern<ToxEventArgs.FriendConnectionStatusEventArgs>(h => tox.OnFriendConnectionStatusChanged += h, h => tox.OnFriendConnectionStatusChanged -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FriendMessageEventArgs> Message
                => Observable.FromEventPattern<ToxEventArgs.FriendMessageEventArgs>(h => tox.OnFriendMessageReceived += h, h => tox.OnFriendMessageReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FriendRequestEventArgs> Request
                => Observable.FromEventPattern<ToxEventArgs.FriendRequestEventArgs>(h => tox.OnFriendRequestReceived += h, h => tox.OnFriendRequestReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.NameChangeEventArgs> NameChanged
                => Observable.FromEventPattern<ToxEventArgs.NameChangeEventArgs>(h => tox.OnFriendNameChanged += h, h => tox.OnFriendNameChanged -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.StatusMessageEventArgs> StatusMessageChanged
                => Observable.FromEventPattern<ToxEventArgs.StatusMessageEventArgs>(h => tox.OnFriendStatusMessageChanged += h, h => tox.OnFriendStatusMessageChanged -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.StatusEventArgs> StatusChanged
                => Observable.FromEventPattern<ToxEventArgs.StatusEventArgs>(h => tox.OnFriendStatusChanged += h, h => tox.OnFriendStatusChanged -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.TypingStatusEventArgs> TypingChanged
                => Observable.FromEventPattern<ToxEventArgs.TypingStatusEventArgs>(h => tox.OnFriendTypingChanged += h, h => tox.OnFriendTypingChanged -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FriendPacketEventArgs> LossyPacketReceived
                => Observable.FromEventPattern<ToxEventArgs.FriendPacketEventArgs>(h => tox.OnFriendLossyPacketReceived += h, h => tox.OnFriendLossyPacketReceived -= h)
                             .Select(x => x.EventArgs);

            public IObservable<ToxEventArgs.FriendPacketEventArgs> LosslessPacketReceived
                => Observable.FromEventPattern<ToxEventArgs.FriendPacketEventArgs>(h => tox.OnFriendLosslessPacketReceived += h, h => tox.OnFriendLosslessPacketReceived -= h)
                             .Select(x => x.EventArgs);
        }
    }
}
