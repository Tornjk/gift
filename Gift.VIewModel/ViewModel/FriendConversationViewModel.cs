using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Gift.ViewModel
{
    public sealed class FriendConversationViewModel : ViewModelBase
    {
        public FriendConversationViewModel(Tox tox, int friendNumber)
        {
            var messages = new SourceList<MessageViewModel>();

            messages.Connect()
                .AutoRefresh(x => x.Read)
                .Filter(x => !x.Read)
                .AsObservableList()
                .CountChanged
                .ToPropertyEx(this, x => x.UnreadCount);

            this.SendMessage = ReactiveCommand.Create<string>(msg =>
            {
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    _ = tox.SendMessage(friendNumber, msg, ToxMessageType.Message);
                    messages.Add(new MessageViewModel("Me", msg, DateTime.Now.Ticks));
                }
            });
            ReadOnlyObservableCollection<MessageViewModel> msgs;
            messages.Connect()
                .Bind(out msgs)
                .Subscribe();

            this.Messages = msgs;

            this.SendTypedMessage = ReactiveCommand.Create(() =>
            {
                var msg = this.InputMessage;
                this.InputMessage = string.Empty;
                return msg;
            }, this.WhenAnyValue(x => x.InputMessage, (string s) => !string.IsNullOrWhiteSpace(s)));

            this.WhenAnyValue(x => x.InputMessage)
                .Select(msg => !string.IsNullOrWhiteSpace(msg))
                .Where(x => x)
                .Do(_ => tox.SetTypingStatus(friendNumber, true))
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => tox.SetTypingStatus(friendNumber, false));

            tox.Events().Friends
                .Message
                .Where(x => x.FriendNumber == friendNumber && x.MessageType == ToxMessageType.Message)
                .Select(x => x.Message)
                .Subscribe(msg => messages.Add(new MessageViewModel(tox.GetFriendName(friendNumber), msg, DateTime.Now.Ticks)));

            this.WhenAnyObservable(x => x.SendTypedMessage)
                .InvokeCommand(this, x => x.SendMessage);
        }

        [Reactive]
        public string InputMessage { get; set; }

        [ObservableAsProperty]
        public int UnreadCount { get; }

        public ReactiveCommand<Unit, string> SendTypedMessage { get; }

        public ReactiveCommand<string, Unit> SendMessage { get; }

        public ReadOnlyObservableCollection<MessageViewModel> Messages { get; }
    }

    public class MessageViewModel : ReactiveObject
    {
        [Reactive]
        public bool Read { get; private set; }

        public long Timestamp { get; }

        public string From { get; }

        public string Message { get; }

        public ReactiveCommand<Unit, Unit> ReadMessage { get; }

        public MessageViewModel(string from, string message, long timestamp)
        {
            this.From = from ?? throw new ArgumentNullException(nameof(from));
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.Timestamp = timestamp;
            this.ReadMessage = ReactiveCommand.Create(() => { this.Read = true; }, this.WhenAnyValue(x => x.Read).Select(x => !x));
        }
    }
}
