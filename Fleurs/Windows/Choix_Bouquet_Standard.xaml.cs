using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Logique d'interaction pour Choix_Bouquet_Standard.xaml
    /// </summary>
    public partial class Choix_Bouquet_Personnalise : UserControl
    {
        private string emailPage;
        private string typePage;
        private string statutPage;

        public Choix_Bouquet_Personnalise(string email, string type)
        {
            emailPage = email;
            typePage= type;
            InitializeComponent(); 
            ;
        }

        private void FinaliseCommande_Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateDeLivraison_DP.SelectedDate;
            DateTime? date_du_jour = DateTime.Now;
            TimeSpan? duree;
            string dureeEnJ_str;
            int dureeEnJ_int;
            bool futur = !(selectedDate < date_du_jour);
            if (selectedDate.HasValue)
            {
                duree = selectedDate - date_du_jour;
                dureeEnJ_str = duree.Value.ToString("dd");
                dureeEnJ_int = Convert.ToInt32(dureeEnJ_str) + 1;
                futur = !(selectedDate < date_du_jour);
                if (futur && dureeEnJ_int>=3)
                {
                    Finalisation_Commande fin_de_commande = new Finalisation_Commande(emailPage, typePage);
                    this.Content = fin_de_commande;
                    //MessageBox.Show("Commande Validé", "Good", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (dureeEnJ_int <=2 && futur)
                {
                    if (MessageBox.Show("Votre commande à été effectué moins de trois jours avant la date de livraison. Il est possible que nous ayons des problèmes de stocks. Voulez vous quand même poursuivre votre commande ?", "Risque de pénurie", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        if (1==2)
                        {

                        }
                        Finalisation_Commande fin_de_commande = new Finalisation_Commande(emailPage, typePage);
                        this.Content = fin_de_commande;
                    }
                }
                else { MessageBox.Show("Veuillez choisir une date de livraison dans le futur", "Date de livraison incorrect", MessageBoxButton.OK, MessageBoxImage.Information); }
                
            }
            else{MessageBox.Show("Veuillez choisir une date de livraison", "Pas de date de livraison", MessageBoxButton.OK, MessageBoxImage.Information);}
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
            this.Content = choix_du_type_de_bouquet;//Peut être utiliser le même qu'au début
        }
    }
}
