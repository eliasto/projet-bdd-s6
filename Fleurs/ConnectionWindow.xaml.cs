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
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
            //connectionString = "SERVER=localhost;PORT=3306;DATABASE=Fleurs;UID=root;PASSWORD=root;";
            connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=marcgroszizi1789;";
            connection = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();
            //this.Content = new Home();
        }

        private void Connection_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //connection.Open();
                string email = Email_TextBox.Text;
                string password = Mdp_TextBox.Password;
                Trace.WriteLine(password);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM clients WHERE email='{email}';";
                MySqlDataReader reader = command.ExecuteReader();
                int compteur = 0;
                while (reader.Read())
                {
                    compteur++;
                    try
                    {
                        string password_mysql = (string)reader["password"];
                        string name_mysql = (string)reader["name"];
                        bool employee = (bool)reader["employee"];


                        if (password_mysql == password)
                        {
                        if(employee)
                            {
                                this.Content = new Home();
                            }
                            else
                            {
                                this.Content = new Choix_Perso_standard(email);
                            }
                            MessageBox.Show("Bonjour " + name_mysql + " ! Bienvenue sur l'extranet de Chez Rose.", "Vous êtes connecté", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Le mot de passe ne correspond pas au compte spécififé.\nPour rénitialiser votre mot de passe, veuillez contacter votre fleuriste.", "Mot de passe incorrect", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
                reader.Close();
                if (compteur == 0)
                {
                    var result = MessageBox.Show("Le compte associé à " + email + " n'existe pas sur notre extranet.\nSouhaitez-vous créer un compte avec cette adresse e-mail ?", "Compte inconnu", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Content = new Register();
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void Redirect_Register_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Register();
        }
    }
}
       