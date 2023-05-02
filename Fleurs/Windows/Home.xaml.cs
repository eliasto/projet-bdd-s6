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

using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Globalization;
using MaterialDesignThemes.Wpf;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        string connectionString;
        MySqlConnection connection;


        public Home()
        {
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            connection = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();


            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT c.id, c.name, c.surname, c.email, c.phone, CASE WHEN COUNT(o.client_id) >= 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'OR' WHEN COUNT(o.client_id) < 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'Bronze' ELSE 'Aucun' END AS fidelity_level FROM clients c LEFT JOIN orders o ON c.id = o.client_id GROUP BY c.id, c.name, c.surname, c.email, c.phone";
            MySqlDataReader reader = command.ExecuteReader();

            List<Client> clients = new List<Client>();

            while (reader.Read())
            {
                Client client = new Client((int)reader["id"], (string)reader["name"], (string)reader["surname"], (string)reader["phone"], (string)reader["email"], (string)reader["fidelity_level"]);
                clients.Add(client);
            }

            dgClients.ItemsSource = clients;

            connection.Close();
            connection.Open();


            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = $"SELECT s.id, s.city, COALESCE(SUM(p.price * op.quantity), 0) AS ProductsRevenue, COALESCE(SUM(b.price * ob.quantity), 0) AS BouquetsRevenue, COALESCE(SUM(f.price * ofl.quantity), 0) AS FlowersRevenue, COALESCE(SUM(p.price * op.quantity), 0) + COALESCE(SUM(b.price * ob.quantity), 0) + COALESCE(SUM(f.price * ofl.quantity), 0) AS TotalRevenue FROM orders o LEFT JOIN order_products op ON o.id = op.order_id LEFT JOIN products p ON op.product_id = p.id LEFT JOIN order_bouquets ob ON o.id = ob.order_id LEFT JOIN bouquets b ON ob.bouquet_id = b.id LEFT JOIN order_flowers ofl ON o.id = ofl.order_id LEFT JOIN flowers f ON ofl.flower_id = f.id JOIN shops s ON s.id = o.shop GROUP BY s.id, s.city ORDER BY TotalRevenue DESC;";

            MySqlDataReader reader1 = command1.ExecuteReader();
            generateBestShop(reader1);
            }

        public async void generateBestShop(MySqlDataReader reader1)
        {
            // Exécutez la tâche de manière asynchrone en utilisant Task.Run

            List<Shop> shops = new List<Shop>();

            while (reader1.Read())
            {
                string photoUrl = await Task.Run(() => GetCityPhoto((string)reader1["city"]));
                Shop shop = new Shop((int)reader1["id"], (string)reader1["city"], (decimal)reader1["TotalRevenue"], (decimal)reader1["FlowersRevenue"], (decimal)reader1["ProductsRevenue"], (decimal)reader1["BouquetsRevenue"], photoUrl);
                shops.Add(shop);
                Trace.WriteLine(shops[0].image);
                this.best_shop_image.Source = new BitmapImage(new Uri(shops[0].image));
                this.best_shop_text.Text = shops[0].city+"\nTotal accessories sold: "+shops[0].productsRevenue+"€\nTotal flowers sold: "+shops[0].flowersRevenue+"€\nTotal bouquets sold: "+shops[0].bouquetsRevenue+"€\nTurnover: "+shops[0].turnover+"€";
                Trace.WriteLine("TESSSSSSSSSSSST");
            }
        }

        private void Show_History(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;

            Trace.WriteLine(id);
        }

        async private void Export_JSON(object sender, RoutedEventArgs e)
        {
            

        }

        public static async Task<string> GetCityPhoto(string cityName)
        {
            // Remplacez "YOUR_API_KEY" par votre clé d'API Google Places
            string apiKey = "AIzaSyBydPR-PsIZ-H4BvAfZRbh5V8Zgxw2hDpw";

            // Encodez le nom de la ville pour l'utiliser dans l'URL de l'API
            string encodedCityName = System.Web.HttpUtility.UrlEncode(cityName);

            // Construisez l'URL de l'API Place Photos
            string apiUrl = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={encodedCityName}&inputtype=textquery&fields=photos&key={apiKey}";

            // Effectuez une requête HTTP GET pour récupérer les informations de la ville
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                // Analysez la réponse JSON pour récupérer l'URL de la photo
                var json = JObject.Parse(content);
                var photos = json["candidates"][0]["photos"];
                if (photos != null && photos.Count() > 0)
                {
                    var photoRef = photos[0]["photo_reference"].ToString();
                    return $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=2000&photoreference={photoRef}&key={apiKey}";
                }
                else
                {
                    return null;
                }
            }
        }

        private void Export_XML(object sender, RoutedEventArgs e)
        {

        }
    }

    

    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string fidelity_level { get; set; } //0: no fidelity, 1: bronze, 2: gold


        public Client(int id, string name, string surname, string phone, string email, string fidelity_level)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.phone = phone;
            this.email = email;
            this.fidelity_level = fidelity_level;
        }


    }

    public class Shop
    {
        public int id { get; set; }
        public string city { get; set; }
        public decimal turnover { get; set; }
        public decimal flowersRevenue { get; set; }
        public decimal productsRevenue { get; set; }
        public decimal bouquetsRevenue { get; set; }
        public string image { get; set; }

        public Shop(int id, string city, decimal turnover, decimal flowersRevenue, decimal productsRevenue, decimal bouquetsRevenue, string image)
        {
            this.id = id;
            this.city = city;
            this.turnover = turnover;
            this.flowersRevenue = flowersRevenue;
            this.productsRevenue = productsRevenue;
            this.bouquetsRevenue = bouquetsRevenue;
            this.image = image;
        }
    }


}
