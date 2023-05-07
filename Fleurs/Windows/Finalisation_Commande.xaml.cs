using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        int bouquet_idPage;

        List<Shop> shops = new List<Shop>();

        string connectionString = new Utils.Utils().connectionString;
        MySqlConnection connection;
        public Finalisation_Commande(string email, string type, string bouquet, string etat, string dateDeLivraison, decimal prix, int bouquet_id)
        {
            emailPage = email;
            typePage = type;
            bouquetPage = bouquet;
            etatPage = etat;
            dateDeLivraisonPage = dateDeLivraison;
            bouquet_idPage = bouquet_id;
            prixPage = Convert.ToDouble(prix);
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            connection.Open();

            shops = new List<Shop>();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM shops;";
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Shop shop = new Shop((int)reader["id"], (string)reader["city"]);
                shops.Add(shop);
            }
            reader.Close();
            List<string> Nom_shop = new List<string>();
            for (int i = 0; i < shops.Count; i++)
            {
                Nom_shop.Add(shops[i].City);
            }
            ChoixM_ComboBox.ItemsSource = Nom_shop;
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
            connection = new MySqlConnection(connectionString);
            connection.Open();

            shops = new List<Shop>();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM shops;";
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Shop shop = new Shop((int)reader["id"], (string)reader["city"]);
                shops.Add(shop);
            }
            reader.Close();
            List<string> Nom_shop = new List<string>();
            for (int i = 0; i < shops.Count; i++)
            {
                Nom_shop.Add(shops[i].City);
            }
            ChoixM_ComboBox.ItemsSource = Nom_shop;
        }
        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            if (typePage == "CS")
            {
                Choix_Bouquet_Standard choix_standard = new Choix_Bouquet_Standard(emailPage, typePage);
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
            else if (string.IsNullOrEmpty(ChoixM_ComboBox.Text))
            {
                MessageBox.Show("Veuillez entrer un magasin", "Message invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                            //Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            //Fleurs.MainWindow mainWindow = new Fleurs.MainWindow();
                            //this.Content = mainWindow.Content;
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
            int id_order = -1;
            int address_exist = -1;
            int id_address = -1;
            int id_shop = -1;
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = $"UPDATE bouquets " +
                    $"SET stock = stock-1 " +
                    $"WHERE name='{bouquetPage}';";
            reader = command.ExecuteReader();
            reader.Close();

            command.CommandText = $"SELECT COUNT(*) FROM address WHERE complement='{Complement_TextBox.Text}' AND street='{Adresse_TextBox.Text}'  AND city='{Ville_TextBox.Text}' AND postal_code='{CodePostale_TextBox.Text}';";
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                address_exist = reader.GetInt32(0);
            }
            reader.Close();
            if (address_exist<1)
            {
                command.CommandText = $"INSERT INTO address (complement, street, city, postal_code)" +
                    $" VALUES('{Complement_TextBox.Text}', '{Adresse_TextBox.Text}', '{Ville_TextBox.Text}', {CodePostale_TextBox.Text});";
                reader = command.ExecuteReader();
                reader.Close();
            }
            command.CommandText = $"SELECT id FROM address WHERE " +
                $"complement='{Complement_TextBox.Text}' AND street='{Adresse_TextBox.Text}' " +
                $"AND city='{Ville_TextBox.Text}' AND postal_code={CodePostale_TextBox.Text};";

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                id_address = reader.GetInt32(0);
            }
            reader.Close();


            for (int i = 0; i < shops.Count; i++)
            {
                if (shops[i].City == ChoixM_ComboBox.Text)
                {
                    id_shop = shops[i].Id;
                }
            }

            if (typePage == "CS")
            {
                command.CommandText = $"INSERT INTO orders " +
                    $"(client_id, address, delivery, max_price, message, status, type, shop) " +
                    $"VALUES({id},{id_address},'{dateDeLivraisonPage}',{prix.ToString().Replace(",",".")},'{Message}','{etatPage}','{typePage}',{id_shop});";//voir pour les address
                reader = command.ExecuteReader();
                reader.Close();
                command.CommandText = $"SELECT id FROM orders WHERE " +
                    $"client_id={id} AND address={id_address} AND delivery='{dateDeLivraisonPage}' AND max_price={prix.ToString().Replace(",", ".")} AND " +
                    $"message='{Message}' AND status='{etatPage}' AND type='{typePage}'";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id_order = reader.GetInt32(0);
                }
                reader.Close();
                command.CommandText = $"INSERT INTO order_bouquets (order_id, bouquet_id, quantity) VALUES({id_order},{bouquet_idPage},1)";//Ajt le bouquet_id
                reader = command.ExecuteReader();
                reader.Close();

            }
            else
            {
                command.CommandText = $"INSERT INTO orders " +
                    $"(client_id, address, delivery, max_price, message, status, type, wishes, shop) " +
                    $"VALUES({id},{id_address},'{dateDeLivraisonPage}',{prix.ToString().Replace(",", ".")},'{Message}','{etatPage}','{typePage}','{wishPage}', {id_shop});";//voir pour les address
                reader = command.ExecuteReader();
                reader.Close();
            }
        }
        public int finfIdClient()//faire la même avec tout les id
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
        private class Shop
        {
            public int Id { get; set; }
            public string City { get; set; }
            public Shop(int id, string city)
            {
                this.Id = id;
                this.City = city;
            }


        }
    }
}
