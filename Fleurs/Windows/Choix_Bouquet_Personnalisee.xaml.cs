using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Choix_Bouquet_Personnalisee.xaml
    /// </summary>
    public partial class Choix_Bouquet_Personnalisee : UserControl
    {
        //regex function so the input can only contain number from 0 to 9 and a dot
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private string emailPage;
        private string typePage;
        public Choix_Bouquet_Personnalisee(string email, string type)
        {
            emailPage = email;
            typePage = type;
            InitializeComponent();
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
            this.Content = choix_du_type_de_bouquet;
        }
        //Check if every field has the right type of value and is not empty, then passes to the next page with the right values
        private void FinaliseCommande_Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateDeLivraison_DP.SelectedDate;
            DateTime? date_du_jour = DateTime.Now;
            TimeSpan? duree;
            string dureeEnJ_str;
            int dureeEnJ_int;
            bool futur = !(selectedDate < date_du_jour);
            string dateDeLivraison;
            string wish;
            double budget;
            if (selectedDate.HasValue && !string.IsNullOrEmpty(Wish_TextBox.Text))
            {
                dateDeLivraison = selectedDate.Value.ToString("yyyy-MM-dd");
                duree = selectedDate - date_du_jour;
                dureeEnJ_str = duree.Value.ToString("dd");
                dureeEnJ_int = Convert.ToInt32(dureeEnJ_str) + 1;
                futur = !(selectedDate < date_du_jour);
                wish = Wish_TextBox.Text;
                try
                {
                    budget = double.Parse(Budget_TextBox.Text, CultureInfo.InvariantCulture);
                    if (futur /*&& dureeEnJ_int >= 3*/)
                    {
                        Finalisation_Commande fin_de_commande = new Finalisation_Commande(emailPage, typePage, wish, "CPAV", dateDeLivraison, budget);
                        this.Content = fin_de_commande;
                    }
                    else { MessageBox.Show("Veuillez choisir une date de livraison dans le futur", "Date de livraison incorrect", MessageBoxButton.OK, MessageBoxImage.Warning); }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Le Budget n'est pas dans le bon format", "Erreur budget", MessageBoxButton.OK, MessageBoxImage.Warning);
                }                
            }
            else if (!selectedDate.HasValue)
            {
                MessageBox.Show("Veuillez choisir une date de livraison", "Pas de date de livraison", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Veuillez choisir une description pour votre arrangement floral", "Pas de description", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        // If a client already knows what they want they can pass directly to a page where they choose from the flower and products db
        private void NoWish_Button_Click(object sender, RoutedEventArgs e)
        {
            Bouquet_Perso_NoWish Skip = new Bouquet_Perso_NoWish(emailPage, typePage);
            this.Content = Skip;
        }
    }
}
