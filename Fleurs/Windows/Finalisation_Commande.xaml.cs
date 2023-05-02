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
using System.Reflection.PortableExecutable;
using System.Globalization;

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
        double prixPage;
        double budgetPage;
        string Message;
        string reduction;
        string wishPage;

        string connectionString;
        MySqlConnection connection;
        public Finalisation_Commande(string email, string type, string bouquet, string etat, string dateDeLivraison, float prix)
        {
            emailPage = email;
            typePage = type;
            bouquetPage = bouquet;
            etatPage = etat;
            dateDeLivraisonPage = dateDeLivraison;
            prixPage = Convert.ToDouble(prix);
            InitializeComponent();
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            //connectionString = "SERVER=localhost;PORT=3306;DATABASE=Fleurs;UID=root;PASSWORD=root;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }
        public Finalisation_Commande(string email, string type, string wish, string etat, string dateDeLivraison, double budget)
        {
            emailPage = email;
            typePage = type;
            etatPage = etat;
            dateDeLivraisonPage = dateDeLivraison;
            budgetPage = budget;
            wishPage = wish;
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
            if (string.IsNullOrEmpty(Adresse_TextBox.Text))
            {
                MessageBox.Show("Veuillez entrer une adresse","Adresse invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(Ville_TextBox.Text))
            {
                MessageBox.Show("Veuillez entrer une ville", "Ville invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(CodePostale_TextBox.Text))
            {
                MessageBox.Show("Veuillez entrer un code postale", "Code postale invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if(string.IsNullOrEmpty(Message_TextBox.Text))
            {
                MessageBox.Show("Veuillez entrer un message", "Message invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Payement();
            }            
            //juste ça 
            // et relire et commenter
        }
        public void Payement()
        {
            double prix;
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            if (MessageBox.Show("Voulez vous effectuer le payement ?", "Confirmation de payement", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Message = Message_TextBox.Text;
                command.CommandText = $"SELECT fidelity_level FROM(SELECT c.id, c.name, c.surname, c.email, c.phone, " +
                        $"CASE    " +
                            $"WHEN COUNT(o.client_id) >= 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'OR' " +
                            $"WHEN (COUNT(o.client_id) < 5 AND COUNT(o.client_id) >=1) AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'BRONZE' " +//pas sur
                            $"ELSE 'Aucun' " +
                    $"END AS fidelity_level " +
                    $"FROM clients c " +
                    $"LEFT JOIN orders o ON c.id = o.client_id " +
                    $"GROUP BY c.id, c.name, c.surname, c.email, c.phone) " +
                    $"AS subquery WHERE email='{emailPage}';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reduction = reader.GetString(0);//  GetString(0);
                }
                reader.Close();
                if (typePage=="CS")
                {
                    prix = prixPage;
                }
                else
                {
                    prix = budgetPage;
                }
                switch (reduction)
                {
                    case "OR":
                        prix = prix * 0.85;
                        if (MessageBox.Show($"Votre commande à bien été effectué, félicitation vous avez profité d'une réduction Or de 15% sur votre bouquet qui coute maintenant {prix}€. Voulez vous continuez vos achats ?", "Commande complete", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            PayementInfoToDB(prix);
                            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
                            this.Content = choix_du_type_de_bouquet;
                        }
                        else
                        {
                            PayementInfoToDB(prix);
                            Fleurs.MainWindow mainWindow = new Fleurs.MainWindow();
                            this.Content = mainWindow.Content;
                        }
                        break;
                    case "BRONZE":
                        prix = prix * 0.95;
                        if (MessageBox.Show($"Votre commande à bien été effectué, félicitation vous avez profité d'une réduction Bronze de 5% sur votre bouquet qui coute maintenant {prix}€. Voulez vous continuez vos achats ?", "Commande complete", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            PayementInfoToDB(prix);
                            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
                            this.Content = choix_du_type_de_bouquet;
                        }
                        else
                        {
                            PayementInfoToDB(prix);
                            Fleurs.MainWindow mainWindow = new Fleurs.MainWindow();
                            this.Content = mainWindow.Content;
                        }
                        break;
                    default:
                        if (MessageBox.Show("Votre commande à bien été effectué. Voulez vous continuez vos achats ?", "Commande complete", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            PayementInfoToDB(prix);
                            Choix_Perso_standard choix_du_type_de_bouquet = new Choix_Perso_standard(emailPage);
                            this.Content = choix_du_type_de_bouquet;
                        }
                        else
                        {
                            PayementInfoToDB(prix);
                            Fleurs.MainWindow mainWindow = new Fleurs.MainWindow();
                            this.Content = mainWindow.Content;
                        }
                        break;
                }
            }
        }
        public void PayementInfoToDB(double prix)
        {
            int id = finfIdClient();
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = $"UPDATE bouquets " +
                    $"SET stock = stock-1 " +
                    $"WHERE name='{bouquetPage}';";
            reader = command.ExecuteReader();
            reader.Close();
            if (typePage == "CS")
            {
                command.CommandText = $"INSERT INTO orders " +
                    $"(client_id, address, delivery, max_price, message, status, type) " +
                    $"VALUES({id},2,'{dateDeLivraisonPage}',{prix.ToString().Replace(",",".")},'{Message}','{etatPage}','{typePage}');";//voir pour les address
                reader = command.ExecuteReader();
                reader.Close();
            }
            else
            {
                command.CommandText = $"INSERT INTO orders " +
                    $"(client_id, address, delivery, max_price, message, status, type, wishes) " +
                    $"VALUES({id},2,'{dateDeLivraisonPage}',{prix.ToString().Replace(",", ".")},'{Message}','{etatPage}','{typePage}','{wishPage}');";//voir pour les address
                reader = command.ExecuteReader();
                reader.Close();
            }
        }
        public int finfIdClient()
        {
            int id=-1;
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = $"SELECT id FROM clients WHERE email='{emailPage}';";
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                id = reader.GetInt32(0) ;
            }
            reader.Close();
            return id;
        }
    }
}
