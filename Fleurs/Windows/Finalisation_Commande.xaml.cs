using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Text.RegularExpressions;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Finalisation_Commande.xaml
    /// </summary>
    public partial class Finalisation_Commande : UserControl
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        string emailPage;
        string typePage;
        string bouquetPage;
        string etatPage;
        string dateDeLivraisonPage;
        int prixPage;
        string Message;

        string connectionString;
        MySqlConnection connection;
        public Finalisation_Commande(string email, string type, string bouquet, string etat, string dateDeLivraison, int prix)
        {
            emailPage = email;
            typePage = type;
            bouquetPage = bouquet;
            etatPage = etat;
            dateDeLivraisonPage = dateDeLivraison;
            prixPage = prix;
            InitializeComponent();
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            //connectionString = "SERVER=localhost;PORT=3306;DATABASE=Fleurs;UID=root;PASSWORD=root;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
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
    private void Payement_Button_Click(object sender, RoutedEventArgs e)
        {
            Message = Message_TextBox.Text;
            
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE bouquets " +
                $"SET stock = stock-1 " +
                $"WHERE name='{bouquetPage}';";
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.CommandText = $"INSERT INTO orders " +
                $"(address, delivery, max_price, message, status, type) " +
                $"VALUES(2,'{dateDeLivraisonPage}',{prixPage},'{Message}','{etatPage}','{typePage}');";//voir pour les address
            reader = command.ExecuteReader();
            reader.Close();
            //juste ça 
            // et relire et commenter
        }
    }
}
