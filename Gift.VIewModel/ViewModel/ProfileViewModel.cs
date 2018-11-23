using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SharpTox.Core;
using System;
using System.Reactive.Linq;

namespace Gift.ViewModel
{
    public sealed class ProfileViewModel : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string StatusMessage { get; set; }

        public ProfileViewModel(Tox tox)
        {
            this.Name = tox.Name;
            this.StatusMessage = tox.StatusMessage;

            this.WhenAnyValue(x => x.Name)
                .Throttle(TimeSpan.FromSeconds(2))
                .BindTo(tox, x => x.Name);

            this.WhenAnyValue(x => x.StatusMessage)
                .Throttle(TimeSpan.FromSeconds(2))
                .BindTo(tox, x => x.StatusMessage);
        }
    }
}
