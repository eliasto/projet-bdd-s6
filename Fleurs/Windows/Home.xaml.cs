using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using System.Text.Json;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        string connectionString = new Utils.Utils().connectionString;
        MySqlConnection connection;
        MySqlConnection connection1;
        List<Client> clients = new List<Client>();
        List<Orders> orders = new List<Orders>();

        public Home()
        {
            connection = new MySqlConnection(connectionString);
            connection1 = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();


            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT c.id, c.name, c.surname, c.email, c.phone, CASE WHEN COUNT(o.client_id) >= 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'OR' WHEN COUNT(o.client_id) < 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'Bronze' ELSE 'Aucun' END AS fidelity_level FROM clients c LEFT JOIN orders o ON c.id = o.client_id GROUP BY c.id, c.name, c.surname, c.email, c.phone";
            MySqlDataReader reader = command.ExecuteReader();

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

            reader.Close();
            connection1.Open();
            command = connection1.CreateCommand();
            //Bouquet qui a le plus de succès
            command.CommandText = $"SELECT bouquets.name, SUM(order_bouquets.quantity) AS total_sales FROM bouquets JOIN order_bouquets ON bouquets.id = order_bouquets.bouquet_id JOIN orders ON order_bouquets.order_id = orders.id WHERE orders.type = 'CS' GROUP BY bouquets.id ORDER BY total_sales DESC LIMIT 1;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string bouquet_name = (string)reader["name"];
                decimal total_sales = (decimal)reader["total_sales"];
                this.best_bouquet_title.Text = bouquet_name;
                this.best_bouquet_text.Text = "Le bouquet qui a le plus de succès est " + bouquet_name + ", avec un total de " + total_sales + " ventes.";
                this.best_bouquet_image.Source = new BitmapImage(new Uri("https://images.squarespace-cdn.com/content/v1/5a2d9772914e6b4aaeea0989/1604229470337-D6D73ZNU3HB7Z1U2F3PT/BOYA-fleurs-france-horticulteur-paris.jpg?format=1000w"));
            }

            reader.Close();
            command = connection1.CreateCommand();
            //Calcul du prix moyen d'un bouquet
            command.CommandText = $"SELECT AVG(bouquets.price) FROM orders JOIN order_bouquets ON orders.id = order_bouquets.order_id JOIN bouquets ON order_bouquets.bouquet_id = bouquets.id;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                decimal average = reader["AVG(bouquets.price)"] == DBNull.Value ? 0 : (decimal)reader["AVG(bouquets.price)"];
                this.average_price_bouquet_title.Text = "Prix moyen d'un bouquet";
                this.average_price_bouquet_text.Text = "Le prix moyen d'un bouquet d'un panier type d'un client est de "+ average+" €.";
                this.average_price_bouquet_image.Source = new BitmapImage(new Uri("https://i.pinimg.com/736x/e7/99/b9/e799b9b79a896eeec05b1fff13d86f91.jpg"));
            }

            reader.Close();
            command = connection1.CreateCommand();
            //Fleur exotique la moins vendue
            command.CommandText = $"SELECT flowers.name, SUM(order_flowers.quantity) as total_sale FROM orders JOIN order_flowers ON order_flowers.order_id = orders.id JOIN flowers on order_flowers.flower_id = flowers.id GROUP BY flowers.name ORDER BY total_sale ASC LiMIT 1;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = (string)reader["name"];
                decimal total_sale = (decimal)reader["total_sale"];
                this.worst_flower_title.Text = name;
                this.worst_flower_text.Text = "La fleur avec le moins de vente est la "+name+" avec "+total_sale+" ventes.";
                this.worst_flower_image.Source = new BitmapImage(new Uri("https://jardinage.lemonde.fr/images/dossiers/2018-01/fleurs-fanees-171021.jpg"));
            }

            string text = "";

            reader.Close();
            command = connection1.CreateCommand();
            //Meilleur client dans l'année
            command.CommandText = $"SELECT clients.id, clients.name, SUM(bouquets.price * order_bouquets.quantity) AS total_bouquets FROM orders JOIN clients ON orders.client_id = clients.id JOIN order_bouquets ON orders.id = order_bouquets.order_id JOIN bouquets ON order_bouquets.bouquet_id = bouquets.id WHERE YEAR(orders.creation_date) = YEAR(CURRENT_DATE()) GROUP BY clients.id, clients.name ORDER BY total_bouquets DESC LIMIT 1;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = (string)reader["name"];
                decimal total_bouquets = (decimal)reader["total_bouquets"];
                text += "Le meilleur client dans l'année est "+name+" où il a acheté pour "+total_bouquets+"€ de bouquets.";
            }

            reader.Close();
            command = connection1.CreateCommand();
            //Meilleur client dans le mois
            command.CommandText = $"SELECT clients.id, clients.name, SUM(bouquets.price * order_bouquets.quantity) AS total_bouquets FROM orders JOIN clients ON orders.client_id = clients.id JOIN order_bouquets ON orders.id = order_bouquets.order_id JOIN bouquets ON order_bouquets.bouquet_id = bouquets.id WHERE MONTH(orders.creation_date) = MONTH(CURRENT_DATE()) GROUP BY clients.id, clients.name ORDER BY total_bouquets DESC LIMIT 1;";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = (string)reader["name"];
                decimal total_bouquets = (decimal)reader["total_bouquets"];
                text += "Le meilleur client dans le mois est " + name + " où il a acheté pour " + total_bouquets + "€ de bouquets.";
            }

            this.best_client_title.Text = "Les meilleurs clients";
            this.best_client_text.Text = text;
            this.best_client_image.Source = new BitmapImage(new Uri("https://blog.smile.io/content/images/2018/Do%20You%20Know%20Who%20Your%20Best%20Customers%20Are/Best-Customers-Feature.png"));
            reader.Close();
            List<Products> products = new List<Products>();

            command = connection1.CreateCommand();
            command.CommandText = $"SELECT * FROM products;";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                string type = "Accessoires";
                string description = (string)reader["description"];
                decimal price = (decimal)reader["price"];
                int stock = (int)reader["stock"];
                int start_month = 1;
                int end_month = 12;
                string category = "Aucune catégorie";
                bool alert = (bool)reader["alert"];
                products.Add(new Products(type, name,description,stock,price, start_month, end_month, category, alert));
            }
            reader.Close();

            command = connection1.CreateCommand();
            command.CommandText = $"SELECT * FROM flowers;";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                string type = "Fleurs";
                string description = "Aucune description";
                decimal price = (decimal)reader["price"];
                int stock = (int)reader["stock"];
                int start_month = (int)reader["start_month"];
                int end_month = (int)reader["end_month"];
                string category = "Aucune catégorie";
                bool alert = (bool)reader["alert"];
                products.Add(new Products(type, name, description, stock, price, start_month, end_month, category, alert));
            }
            reader.Close();


            command = connection1.CreateCommand();
            command.CommandText = $"SELECT bouquets.name, bouquets.description, bouquets.price, bouquets.stock, bouquets.alert, category.name AS category FROM Fleurs.bouquets JOIN category ON bouquets.category = category.id;";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                string type = "Bouquets";
                string description = (string)reader["description"];
                decimal price = (decimal)reader["price"];
                int stock = (int)reader["stock"];
                int start_month = 1;
                int end_month = 12;
                string category = (string)reader["category"];
                bool alert = (bool)reader["alert"];
                products.Add(new Products(type, name, description, stock, price, start_month, end_month, category, alert));
            }
            reader.Close();

            Trace.WriteLine(products.Count());
            products_data.ItemsSource = products;

            command = connection1.CreateCommand();
            command.CommandText = $"SELECT orders.id,orders.client_id,orders.type,orders.wishes,orders.max_price,orders.address,orders.message,orders.delivery,orders.creation_date,orders.status,shops.city as shop FROM orders JOIN shops ON orders.shop = shops.id;";
            reader = command.ExecuteReader();

            while (reader.Read())
            {

                string wishes;
                decimal max_price;
                if (reader["wishes"] == DBNull.Value)
                {
                    wishes = "Aucun voeu"; // ou null, selon vos besoins
                }
                else
                {
                    wishes = (string)reader["wishes"];
                }

                if (reader["max_price"] == DBNull.Value)
                {
                    max_price = 0; // ou null, selon vos besoins
                }
                else
                {
                    max_price = (decimal)reader["max_price"];
                }

                int id = (int)reader["id"];
                int client_id = (int)reader["client_id"];
                string type = (string)reader["type"];
                int address = (int)reader["address"];
                string message = (string)reader["message"];
                DateTime delivery = (DateTime)reader["delivery"];
                DateTime creation_time = (DateTime)reader["creation_date"];
                string status = (string)reader["status"];
                string shop = (string)reader["shop"];
                string content = ContentOfTheOrder(id);

                orders.Add(new Orders(id, client_id, type, wishes, max_price, address, message, delivery, creation_time, status, shop, content));

            }

            orders_data.ItemsSource = orders;

            reader.Close();
            foreach(Client client in clients)
            {
                combobox_clients_orders.Items.Add("#" + client.id + " " + client.name + " " + client.surname);
            }
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
            }
        }

        private void Show_History(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;

            foreach(Client client in clients)
            {
                if(client.id == id)
                {
                    combobox_clients_orders.SelectedItem = "#" + client.id + " " + client.name + " " + client.surname;
                    List<Orders> newOrders = new List<Orders>();
                    foreach(Orders order in orders)
                    {
                        if(order.client_id == id)
                        {
                            newOrders.Add(order);
                        }
                    }

                    orders_data.ItemsSource = newOrders;
                }
            }
            tab_control_home.SelectedIndex = 2;
        }

        private void Validate_Choice_ComboBox(object sender, RoutedEventArgs e)
        {
            string comboboxValue = combobox_clients_orders.SelectedItem.ToString();
            Trace.WriteLine(comboboxValue);
            if(comboboxValue != "System.Windows.Controls.ComboBoxItem : Toutes les commandes")
            {
                string idClientSplitted = comboboxValue.Split('#')[1].Split(' ')[0];
                Trace.WriteLine(idClientSplitted);
                List<Orders> newOrders = new List<Orders>();
                foreach (Orders order in orders)
                {
                    if (order.client_id == Convert.ToInt32(idClientSplitted))
                    {
                        newOrders.Add(order);
                    }
                }

                orders_data.ItemsSource = newOrders;
            }
            else
            {
                orders_data.ItemsSource = orders;
            }

            
               
        }


        private void Export_JSON(object sender, RoutedEventArgs e)
        {
            MySqlCommand command = this.connection1.CreateCommand();
            command.CommandText = $"SELECT c.id, c.name, c.surname, c.phone, c.email, CASE WHEN COUNT(o.client_id) >= 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'OR' WHEN COUNT(o.client_id) < 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'Bronze' ELSE 'Aucun' END AS fidelity_level FROM clients AS c LEFT JOIN orders AS o ON c.id = o.client_id WHERE o.creation_date IS NULL OR o.creation_date < DATE_SUB(NOW(), INTERVAL 6 MONTH) GROUP BY c.id, c.name,c.surname,c.phone,c.email;";
            MySqlDataReader reader = command.ExecuteReader();
            List<Client> clients = new List<Client>();
            while (reader.Read())
            {
                int id = (int)reader["id"];
                string name = (string)reader["name"];
                string surname = (string)reader["surname"];
                string phone = (string)reader["phone"];
                string email = (string)reader["email"];
                string fidelity_level = (string)reader["fidelity_level"];
                Client client = new Client(id, name, surname, phone, email, fidelity_level);
                clients.Add(client);
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(clients, options);

            Trace.WriteLine(json);
            reader.Close();

            // Créez une instance de SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Configurez les propriétés de SaveFileDialog
            saveFileDialog.Filter = "Fichier JSON (*.json)|*.json";
            saveFileDialog.Title = "Sélectionnez un emplacement de sauvegarde";
            saveFileDialog.FileName = "clients.json";

            // Affichez la boîte de dialogue de sauvegarde et vérifiez si l'utilisateur a cliqué sur le bouton "Enregistrer"
            if (saveFileDialog.ShowDialog() == true)
            {
                // Ouvrez le fichier de sauvegarde en utilisant un StreamWriter
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    // Écrivez la variable json dans le fichier
                    streamWriter.Write(json);
                    MessageBox.Show("Le fichier des clients n'ayant pas commandés depuis 6 mois a été enregistré.", "Fichier exporté", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }



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
            MySqlCommand command = this.connection1.CreateCommand();
            command.CommandText = $"SELECT c.id, c.name, c.surname, c.phone, c.email, COUNT(o.id) AS num_orders, CASE WHEN COUNT(o.client_id) >= 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'OR' WHEN COUNT(o.client_id) < 5 AND AVG(DATEDIFF(o.delivery, o.creation_date)) <= 30 THEN 'Bronze' ELSE 'Aucun' END AS fidelity_level FROM clients AS c JOIN orders AS o ON c.id = o.client_id WHERE o.creation_date > DATE_SUB(NOW(), INTERVAL 1 MONTH) GROUP BY c.id, c.name, c.surname, c.phone, c.email HAVING COUNT(o.id) > 1;";
            MySqlDataReader reader = command.ExecuteReader();
            List<Client_XML> clients = new List<Client_XML>();
            while (reader.Read())
            {
                int id = (int)reader["id"];
                string name = (string)reader["name"];
                string surname = (string)reader["surname"];
                string phone = (string)reader["phone"];
                string email = (string)reader["email"];
                string fidelity_level = (string)reader["fidelity_level"];
                Client_XML client = new Client_XML { email = email, id = id, name = name, surname = surname, phone = phone, fidelity_level = fidelity_level};
                clients.Add(client);
            }
            
            reader.Close();
            XmlSerializer xs = new XmlSerializer(typeof(List<Client_XML>));
            // Créez une instance de SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Configurez les propriétés de SaveFileDialog
            saveFileDialog.Filter = "Fichier XML (*.xml)|*.xml";
            saveFileDialog.Title = "Sélectionnez un emplacement de sauvegarde";
            saveFileDialog.FileName = "clients.xml";

            // Affichez la boîte de dialogue de sauvegarde et vérifiez si l'utilisateur a cliqué sur le bouton "Enregistrer"
            if (saveFileDialog.ShowDialog() == true)
            {
                // Ouvrez le fichier de sauvegarde en utilisant un StreamWriter
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    // Écrivez la variable json dans le fichier
                    xs.Serialize(streamWriter, clients);
                    MessageBox.Show("Le fichier des clients ayant commandé plusieurs fois durant le dernier mois a été enregistré.", "Fichier exporté", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }



        }

        private string ContentOfTheOrder (int order_id)
        {
            string content = "";

            MySqlConnection conne = new MySqlConnection(connectionString);
            conne.Open();
            MySqlCommand command = conne.CreateCommand();
            command.CommandText = $"SELECT order_products.quantity, products.name FROM order_products JOIN products ON products.id = order_products.product_id WHERE order_products.order_id = " + order_id + ";";
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                int quantity = (int)reader["quantity"];
                content += "\n" + quantity + "x " + name;
            }

            reader.Close();

            command.CommandText = $"SELECT order_flowers.quantity, flowers.name FROM order_flowers JOIN flowers ON flowers.id = order_flowers.flower_id WHERE order_flowers.order_id = "+order_id+";";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                int quantity = (int)reader["quantity"];
                content += "\n" + quantity + "x " + name;
            }

            reader.Close();

            command.CommandText = $"SELECT order_bouquets.quantity, bouquets.name FROM order_bouquets JOIN bouquets ON bouquets.id = order_bouquets.bouquet_id WHERE order_bouquets.order_id = " + order_id + ";";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["name"];
                int quantity = (int)reader["quantity"];
                content += "\n" + quantity + "x " + name;
            }

            reader.Close();

            return content;

        }

    }

    

    public class Client
    {
        [XmlAttribute]
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string fidelity_level { get; set; } //0: no fidelity, 1: bronze, 2: gold


        public Client(int id, string name, string surname, string phone, string email, string fidelity_level = "Aucun")
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.phone = phone;
            this.email = email;
            this.fidelity_level = fidelity_level;
        }


    }

    public class Client_XML
    {
        [XmlAttribute]
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string fidelity_level { get; set; } //0: no fidelity, 1: bronze, 2: gold

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

    public class Orders
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public string type { get; set; }
        public string wishes { get; set; }
        public decimal max_price { get; set; }
        public int address { get; set; } 
        public string message { get; set; } 
        public DateTime delivery { get; set; }
        public DateTime creation_time { get; set; }
        public string status { get; set; }
        public string shop { get; set; }
        public string content { get; set; }
        
        public Orders(int id, int client_id, string type, string wishes, decimal max_price, int address, string message, DateTime delivery, DateTime creation_time, string status, string shop, string content)
        {
            this.id = id;
            this.client_id = client_id;
            this.type = type;
            this.wishes = wishes;
            this.max_price = max_price;
            this.address = address;
            this.message = message;
            this.delivery = delivery;
            this.creation_time = creation_time;
            this.status = status;
            this.shop = shop;
            this.content = content;
        }
    }

    public class Products
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int stock { get; set; }
        public decimal price { get; set; }
        public int start_month { get; set; }
        public int end_month { get; set; }
        public string category { get; set; }  
        public bool alert { get; set; }
        
        public Products(string type, string name, string description, int stock, decimal price, int start_month, int end_month, string category, bool alert)
        {
            this.type = type;
            this.name = name;
            this.description = description;
            this.stock = stock;
            this.price = price;
            this.start_month = start_month;
            this.end_month = end_month;
            this.category = category;
            this.alert = alert;
        }
        

    }

}
