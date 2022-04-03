using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace RegistCodeBehind
{
    /// <summary>
    /// Interaction logic for DisplayPage.xaml
    /// </summary>
    public partial class DisplayPage : UserControl
    {
        public DisplayPage()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("UserControl_PreviewKeyDownだよ");
            var vm = (DisplayPageViewModel) DataContext;
            if (vm.F10Command.CanExecute())
            {
                vm.F10Command.Execute();
            }
        }
    }
}
