using Fleurs.Windows;
using System.Windows;

namespace Fleurs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Content = new Login();
        }
    }
}
       