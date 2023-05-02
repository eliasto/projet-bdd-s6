using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        string emailPage;
        string typePage;
        string statutPage;
        string bouquet;

        string connectionString;
        MySqlConnection connection;

        public Choix_Bouquet_Personnalise(string email, string type)
        {
            emailPage = email;
            typePage= type;
            InitializeComponent();
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            //connectionString = "SERVER=localhost;PORT=3306;DATABASE=Fleurs;UID=root;PASSWORD=root;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        private void FinaliseCommande_Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateDeLivraison_DP.SelectedDate;
            DateTime? date_du_jour = DateTime.Now;
            TimeSpan? duree;
            string dureeEnJ_str;
            int dureeEnJ_int;
            bool futur = !(selectedDate < date_du_jour);
            string bouquet_choisi;
            string dateDeLivraison;
            if (selectedDate.HasValue && !string.IsNullOrEmpty(Choix_ComboBox.Text))
            {
                dateDeLivraison = selectedDate.Value.ToString("yyyy-MM-dd");
                duree = selectedDate - date_du_jour;
                dureeEnJ_str = duree.Value.ToString("dd");
                dureeEnJ_int = Convert.ToInt32(dureeEnJ_str) + 1;
                futur = !(selectedDate < date_du_jour);
                bouquet_choisi = Choix_ComboBox.Text;
                float prix;
                switch(bouquet_choisi)
                {
                    case "Gros Merci":
                        prix = 45;
                        break;
                    case "L’amoureux":
                        prix = 65;
                        break;
                    case "L’Exotique":
                        prix = 40;
                        break;
                    case "Maman Arrangement":
                        prix = 80;
                        break;
                    case "Vive la mariée":
                        prix = 120;
                        break;
                    default:
                        prix = -1;
                        break;
                        ;
                }
                bool en_stock = false;
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT Count(*) FROM bouquets WHERE stock> 0 AND name = '{bouquet_choisi}';";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    en_stock = reader.GetBoolean(0);//  GetString(0);
                }
                reader.Close();
                if (futur && dureeEnJ_int>=3)
                {                   
                    if(en_stock)
                    {
                        Finalisation_Commande fin_de_commande = new Finalisation_Commande(emailPage, typePage, bouquet_choisi, "CC",dateDeLivraison, prix);
                        this.Content = fin_de_commande;
                    }
                    else
                    {
                        MessageBox.Show("Malheuresement, il ne nous reste plus de "+ bouquet_choisi, "Plus de stock", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else if (dureeEnJ_int <=2 && futur)
                {
                    if (en_stock)
                    {
                        if (MessageBox.Show("Votre commande à été effectué moins de trois jours avant la date de livraison. Il est possible que nous ayons des problèmes de stocks. Voulez vous quand même poursuivre votre commande ?", "Risque de pénurie", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            Finalisation_Commande fin_de_commande = new Finalisation_Commande(emailPage, typePage, bouquet_choisi, "VINV", dateDeLivraison, prix);
                            this.Content = fin_de_commande;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Malheuresement, il ne nous reste plus de " + bouquet_choisi, "Plus de stock", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else { MessageBox.Show("Veuillez choisir une date de livraison dans le futur", "Date de livraison incorrect", MessageBoxButton.OK, MessageBoxImage.Warning); }
                
            }
            else if (!selectedDate.HasValue)
            {
                MessageBox.Show("Veuillez choisir une date de livraison", "Pas de date de livraison", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Veuillez choisir un bouquet", "Pas de bouquet", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
            this.Content = choix_du_type_de_bouquet;
        }
    }
}
