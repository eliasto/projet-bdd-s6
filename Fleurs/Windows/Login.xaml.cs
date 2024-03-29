﻿using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        string connectionString = new Utils.Utils().connectionString;
        MySqlConnection connection;
        public Login()
        {
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
                string hash = "";
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Convertir la chaîne en tableau de bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(password);

                    // Calculer la valeur de hachage SHA256
                    byte[] hashBytes = sha256Hash.ComputeHash(bytes);

                    // Convertir le tableau de bytes en chaîne hexadécimale
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }
                    string hashString = builder.ToString();

                    // Afficher la valeur de hachage
                    hash = hashString;
                }
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


                        if (password_mysql == hash)
                        {
                            if (employee)
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
