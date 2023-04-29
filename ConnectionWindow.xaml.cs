using Fleurs.Windows;
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
using System.Linq;
using MySql.Data.MySqlClient;

namespace Fleurs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        MySqlConnection connection;
        public MainWindow()
        {
            connectionString = "SERVER=localhost;PORT=3306;DATABASE=Fleurs;UID=root;PASSWORD=root;";
            connection = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();
        }

        private void Connection_Button_Click(object sender, RoutedEventArgs e)
        {
            //connection.Open();
            string email = Email_TextBox.Text;
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT Count(*) FROM clients WHERE email='{email}';";
            MySqlDataReader reader = command.ExecuteReader();
            string found = "-1";
            while (reader.Read())
            {
                found = reader.GetString(0);
                //Email_TextBox.Text = found;
            }
            reader.Close();
            if (found == "1")
            {
                MessageBox.Show("It works!", "Super", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var result=MessageBox.Show("Voulez vous créer un compte ?", "Votre email n'est pas dans notre systeme!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Creation_Compte creation_Compte = new Creation_Compte();
                    this.Content = creation_Compte; 
                }
            }
        }
    }
}
