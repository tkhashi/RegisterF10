using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace RegistCodeBehind
{
    public class DisplayPageViewModel
    {
        public ReactivePropertySlim<string> F10Text { get; } = new ReactivePropertySlim<string>("何も押されてないよ");
        public ReactiveCommand F10Command { get; } = new ReactiveCommand();
        private CompositeDisposable _disposable = new CompositeDisposable();

        public DisplayPageViewModel()
        {
            F10Command.Subscribe(OnF10Down)
                .AddTo(_disposable);
        }

        private void OnF10Down()
        {
            F10Text.Value = "F10が押されたよ";
        }
    }
}