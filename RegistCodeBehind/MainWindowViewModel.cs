using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;

namespace RegistCodeBehind
{
    public class MainWindowViewModel
    {

        public ReactivePropertySlim<string> F9Text { get; } = new ReactivePropertySlim<string>("何も押されてないよ");
        public ReactiveCommand F9Command { get; } = new ReactiveCommand();
        private CompositeDisposable _disposable = new CompositeDisposable();

       public MainWindowViewModel()
       {
           F9Command.Subscribe(OnF9Down)
               .AddTo(_disposable);
       }

       private void OnF9Down()
       {
           F9Text.Value = "F9が押されたよ";
       }
    }
}