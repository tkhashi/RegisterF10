using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace RegistCodeBehind
{
    public class KeyboardBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty KeyTypeProperty =
            DependencyProperty.Register(
                nameof(KeyType),
                typeof(Key),
                typeof(KeyboardBehavior),
                new FrameworkPropertyMetadata());

        public static readonly DependencyProperty KeyDownProperty =
            DependencyProperty.Register(
                nameof(KeyDown),
                typeof(ICommand),
                typeof(KeyboardBehavior),
                new FrameworkPropertyMetadata());

        public Key KeyType
        {
            get => (Key) GetValue(KeyTypeProperty);
            set => SetValue(KeyTypeProperty, value);
        }

        public ICommand KeyDown
        {
            get => (ICommand) GetValue(KeyDownProperty);
            set => SetValue(KeyDownProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
            var window = Application.Current.MainWindow;
            if (window == null) return;

            window.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var isF9 = e.Key == KeyType;
            var canExecuteF9Command = KeyDown.CanExecute(null);

            if (isF9 && canExecuteF9Command)
            {
                KeyDown.Execute(null);
            }
        }
    }
}