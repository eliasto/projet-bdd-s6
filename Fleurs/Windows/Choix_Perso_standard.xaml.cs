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
    /// Logique d'interaction pour Choix_Perso_standard.xaml
    /// </summary>
    public partial class Choix_Perso_standard : UserControl
    {
        private string emailPage; 
        public Choix_Perso_standard(string email)
        {
            emailPage=email;
            InitializeComponent();
        }

        private void B_P_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Personnalisee choix_Bouquet_Personnalisee = new Choix_Bouquet_Personnalisee(emailPage, "CP");
            this.Content = choix_Bouquet_Personnalisee;
        }

        private void B_Standard_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Standard choix_Bouquet_Standard = new Choix_Bouquet_Standard(emailPage, "CS");
            this.Content = choix_Bouquet_Standard;
        }
    }
}
