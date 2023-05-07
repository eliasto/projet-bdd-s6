using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Bouquet_Perso_NoWish.xaml
    /// </summary>

    public partial class Bouquet_Perso_NoWish : UserControl
    {
        
        List<Fleur> fleurs = new List<Fleur>();
        List<Produit> produits = new List<Produit>();

        string connectionString = new Utils.Utils().connectionString;
        MySqlConnection connection;

        private string emailPage;
        private string typePage;

        public Bouquet_Perso_NoWish(string email, string type)
        {
            emailPage = email;
            typePage = type;
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
            public int Count { get; set; }


            public Fleur(int id, string name, decimal price, int startMonth, int endMonth, int stock)
            {
                this.Id = id;
                this.Name = name;
                this.Price = price;
                this.StartMonth = startMonth;
                this.EndMonth = endMonth;
                this.Stock = stock;
                this.Count = 0;
            }
        }
        private class Produit
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public int Count { get; set; }



            public Produit(int id, string name, string description, decimal price, int stock)
            {
                this.Id = id;
                this.Name = name;
                this.Description = description;
                this.Price = price;
                this.Stock = stock;
                this.Count = 0;
            }
        }

        private void Retour_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Personnalisee Retour = new Choix_Bouquet_Personnalisee(emailPage, typePage);
            this.Content = Retour;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            
            Fleur fleur_add = (sender as FrameworkElement).DataContext as Fleur;
            for (int i=0; i<fleurs.Count ; i++)
            {
                if (fleurs[i].Name == fleur_add.Name && fleurs[i].Stock>0)
                {
                    fleurs[i].Count++;
                    fleurs[i].Stock--;
                }
            }
            Get_price();
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            Fleur fleur_rmv = (sender as FrameworkElement).DataContext as Fleur;
            for (int i = 0; i < fleurs.Count; i++)
            {
                if (fleurs[i].Name == fleur_rmv.Name && fleurs[i].Count>0)
                {
                    fleurs[i].Count--;
                    fleurs[i].Stock++;
                }
            }
            Get_price();
        }
        public void Get_price()
        {
            float prix = 0;
            for (int i = 0; i < fleurs.Count; i++)
            {
                prix += (float)(fleurs[i].Count * fleurs[i].Price);
            }
            for (int i = 0; i < produits.Count; i++)
            {
                prix += (float)(produits[i].Count * produits[i].Price);
            }

            dgFleurs.Items.Refresh();
            dgProduits.Items.Refresh();
            Prix_TextBlock.Text = "Prix : " + prix + "€";
        }

        private void AddProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            Produit product_add = (sender as FrameworkElement).DataContext as Produit;
            for (int i = 0; i < produits.Count; i++)
            {
                if (produits[i].Name == product_add.Name && produits[i].Stock > 0)
                {
                    produits[i].Count++;
                    produits[i].Stock--;
                }
            }
            Get_price();
        }

        private void RemoveProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            Produit product_rmv = (sender as FrameworkElement).DataContext as Produit;
            for (int i = 0; i < produits.Count; i++)
            {
                if (produits[i].Name == product_rmv.Name && produits[i].Count > 0)
                {
                    produits[i].Count--;
                    produits[i].Stock++;
                }
            }
            Get_price();
        }

        private void FinaliseCommande_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
