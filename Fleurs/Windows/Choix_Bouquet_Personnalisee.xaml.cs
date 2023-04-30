using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Choix_Bouquet_Personnalisee.xaml
    /// </summary>
    public partial class Choix_Bouquet_Personnalisee : UserControl
    {
        private string emailPage;
        private string typePage;
        public Choix_Bouquet_Personnalisee(string email, string type)
        {
            emailPage = email;
            typePage = type;
            InitializeComponent();
        }
    }
}
