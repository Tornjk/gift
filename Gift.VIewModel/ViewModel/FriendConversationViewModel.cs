using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Gift.ViewModel.ViewModel
{
    public class FriendConversationViewModel : ReactiveObject
    {
        public FriendConversationViewModel(Tox tox)
        {
            var messages = new SourceList<MessageViewModel>();

            messages.Connect()
                .AutoRefresh(x => x.Read)
                .Filter(x => !x.Read)
                .AsObservableList()
                .CountChanged
                .ToPropertyEx(this, x => x.UnreadCount);
        }

        [ObservableAsProperty]
        public int UnreadCount { get; }
    }

    public class MessageViewModel : ReactiveObject
    {
        public long Timestamp { get; }

        public string From { get; }

        public string Message { get; }

        [Reactive]
        public bool Read { get; private set; }

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
