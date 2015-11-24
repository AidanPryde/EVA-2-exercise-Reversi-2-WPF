
using System.Windows;

namespace Reversi_WPF.View
{
    /// <summary>
    /// Interaction logic for AboutMessage.xaml
    /// </summary>
    public partial class AboutMessage : Window
    {
        public AboutMessage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
