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

        }

        private void Show_History(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;

            Trace.WriteLine(id);
        }

        private void Export_JSON(object sender, RoutedEventArgs e)
        {
            
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


}
