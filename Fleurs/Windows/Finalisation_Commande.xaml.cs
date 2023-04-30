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
    /// Logique d'interaction pour Finalisation_Commande.xaml
    /// </summary>
    public partial class Finalisation_Commande : UserControl
    {
        private string emailPage;
        private string typePage;
        public Finalisation_Commande(string email, string type)
        {
            emailPage = email;
            typePage = type;
            InitializeComponent();
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            if (typePage == "CS")
            {
                Choix_Bouquet_Personnalise choix_standard = new Choix_Bouquet_Personnalise(emailPage, typePage);
                this.Content = choix_standard;
            }
            else if (typePage == "CP")
            {
                Choix_Bouquet_Personnalisee choix_standard = new Choix_Bouquet_Personnalisee(emailPage, typePage);
                this.Content = choix_standard;
            }
        }
    }
}
