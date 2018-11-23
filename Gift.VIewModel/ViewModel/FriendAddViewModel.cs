using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Gift.ViewModel
{
    public class FriendAddViewModel : ViewModelBase
    {
        public FriendAddViewModel(Tox tox)
        {
            if(tox == null)
            {
                throw new ArgumentNullException(nameof(tox));
            }

            this.Add = ReactiveCommand.Create(() => tox.AddFriend(new ToxId(this.ID), this.Message, out _), this.WhenAnyValue(x => x.ID, (string id) => ToxId.IsValid(id)));
        }

        [Reactive]
        public string ID { get; set; }

        [Reactive]
        public string Message { get; set; } = "Hello from Gift!";

        public ReactiveCommand<Unit, uint> Add { get; }
    }
}
