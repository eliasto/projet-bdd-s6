using MySql.Data.MySqlClient;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Bouquet_Perso_NoWish.xaml
    /// </summary>
    public partial class Bouquet_Perso_NoWish : UserControl
    {
        
        List<Fleur> fleurs = new List<Fleur>();
        List<Produit> produits = new List<Produit>();

        string connectionString;
        MySqlConnection connection;

        private string emailPage;
        private string typePage;

        public Bouquet_Perso_NoWish(string email, string type)
        {
            emailPage = email;
            typePage = type;
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            connection = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM flowers;";
            MySqlDataReader reader = command.ExecuteReader();

            fleurs = new List<Fleur>();

            while (reader.Read())
            {
                Fleur fleur = new Fleur((int)reader["id"], (string)reader["name"], (decimal)reader["price"], (int)reader["start_month"], (int)reader["end_month"], (int)reader["stock"]);
                fleurs.Add(fleur);
            }

            dgFleurs.ItemsSource = fleurs;

            connection.Close();
            connection.Open();
            command.CommandText = "SELECT * FROM products;";
            reader = command.ExecuteReader();

            produits = new List<Produit>();

            while (reader.Read())
            {
                Produit produit = new Produit((int)reader["id"], (string)reader["name"], (string)reader["description"], (decimal)reader["price"], (int)reader["stock"]);
                produits.Add(produit);
            }

            dgProduits.ItemsSource = produits;

            connection.Close();
        }
        private class Fleur
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int StartMonth { get; set; }
            public int EndMonth { get; set; }
            public int Stock { get; set; }


            public Fleur(int id, string name, decimal price, int startMonth, int endMonth, int stock)
            {
                this.Id = id;
                this.Name = name;
                this.Price = price;
                this.StartMonth = startMonth;
                this.EndMonth = endMonth;
                this.Stock = stock;
            }
        }
        private class Produit
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }


            public Produit(int id, string name, string description, decimal price, int stock)
            {
                this.Id = id;
                this.Name = name;
                this.Description = description;
                this.Price = price;
                this.Stock = stock;
            }
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Personnalisee Retour = new Choix_Bouquet_Personnalisee(emailPage, typePage);
            this.Content = Retour;
        }
    }
}
