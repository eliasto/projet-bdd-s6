using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Diagnostics;

using System.Text.RegularExpressions;


namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        string connectionString = new Utils.Utils().connectionString;
        MySqlConnection connection;

        public Register()
        {
            connection = new MySqlConnection(connectionString);
            InitializeComponent();
            connection.Open();

            InitializeComponent();
        }

        private void Redirect_Register_Click(object sender, RoutedEventArgs e)
        {
            //TODO : ça ne fonctionne pas lol
            this.Content = new Login();
        }

        private void next_step_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = "";

                if (this.Credit_card_name_TextBox.Text == null || this.Credit_card_name_TextBox.Text.Length == 0)
                {
                    error += " \n - Nom du porteur de la carte bancaire";
                }
                if (this.Credit_card_number_TextBox.Text == null || this.Credit_card_number_TextBox.Text.Length == 0)
                {
                    error += " \n - Numéro de la carte bancaire";
                }
                if (this.Credit_card_expiry_month_TextBox == null || this.Credit_card_expiry_month_TextBox.Text.Length == 0)
                {
                    error += " \n - Mois d'expiration de la carte bancaire";
                }
                if (this.Credit_card_expiry_year_TextBox == null || this.Credit_card_expiry_year_TextBox.Text.Length == 0)
                {
                    error += " \n - Année d'expiration de la carte bancaire";
                }
                if (this.Credit_card_ccv_TextBox == null || this.Credit_card_ccv_TextBox.Text.Length == 0)
                {
                    error += " \n - Code de sécurité de la carte bancaire";
                }

                if (error.Length > 0)
                {
                    MessageBox.Show("Veuillez remplir les champs suivants : " + error);
                }
                else
                {
                    string address = "INSERT INTO address (complement, street, city, postal_code) VALUES (@complement, @street, @city, @postal_code)";

                    MySqlCommand command_address = new MySqlCommand(address, connection);
                    command_address.Parameters.AddWithValue("@complement", this.Complement_TextBox.Text);
                    command_address.Parameters.AddWithValue("@street", this.Street_TextBox.Text);
                    command_address.Parameters.AddWithValue("@city", this.Ville_TextBox.Text);
                    command_address.Parameters.AddWithValue("@postal_code", Convert.ToInt32(this.Postal_code_TextBox.Text));


                    command_address.ExecuteNonQuery();
                    long id_address = command_address.LastInsertedId;

                    string credit_card = "INSERT INTO credit_card (holder, number, expiry, ccv) VALUES (@holder, @number, @expiry, @ccv)";

                    MySqlCommand command_credit_card = new MySqlCommand(credit_card, connection);
                    command_credit_card.Parameters.AddWithValue("@holder", this.Credit_card_name_TextBox.Text);
                    command_credit_card.Parameters.AddWithValue("@number", this.Credit_card_number_TextBox.Text);
                    command_credit_card.Parameters.AddWithValue("@ccv", this.Credit_card_ccv_TextBox.Text);
                    command_credit_card.Parameters.AddWithValue("@expiry", "20" + this.Credit_card_expiry_year_TextBox.Text + "-" + this.Credit_card_expiry_month_TextBox.Text + "-01");


                    command_credit_card.ExecuteNonQuery();
                    long id_credit_card = command_credit_card.LastInsertedId;

                    Trace.WriteLine("Id cc :" + id_credit_card);

                    string clients = "INSERT INTO clients (name, surname, phone, email, password, address, credit_card) VALUES (@name, @surname, @phone, @email, @password, @address, @credit_card)";
                    MySqlCommand command_clients = new MySqlCommand(clients, connection);
                    command_clients.Parameters.AddWithValue("@name", this.Name_TextBox.Text);
                    command_clients.Parameters.AddWithValue("@surname", this.Surname_TextBox.Text);
                    command_clients.Parameters.AddWithValue("@phone", this.Phone_TextBox.Text);
                    command_clients.Parameters.AddWithValue("@email", this.Email_TextBox.Text);
                    command_clients.Parameters.AddWithValue("@password", this.Mdp_TextBox.Password);
                    command_clients.Parameters.AddWithValue("@address", id_address);
                    command_clients.Parameters.AddWithValue("@credit_card", id_credit_card);

                    int result_clients = command_clients.ExecuteNonQuery();

                    if (result_clients > 0)
                    {
                        Trace.WriteLine(result_clients);
                    }
                    else
                    {
                        return;
                    }


                    MessageBox.Show("Félicitation " + this.Name_TextBox.Text + " ! Votre compte a été créé avec succès !", "Bienvenue sur l'extranet de Chez Rose", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur est survenue", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void next_step_2_Click(object sender, RoutedEventArgs e)
        {
            string error = "";

            if (this.Street_TextBox.Text == null || this.Street_TextBox.Text.Length == 0)
            {
                error += " \n - Numéro et nom de la rue";
            }
            if (this.Ville_TextBox.Text == null || this.Ville_TextBox.Text.Length == 0)
            {
                error += " \n - Ville";
            }
            if (this.Postal_code_TextBox == null || this.Postal_code_TextBox.Text.Length == 0)
            {
                error += " \n - Code postal";
            }

            if (error.Length > 0)
            {
                MessageBox.Show("Veuillez remplir les champs suivants : " + error);
            }
            else
            {
                this.address.Visibility = Visibility.Hidden;
                this.credit_card.Visibility = Visibility.Visible;
            }
        }

        private void next_step_1_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            if (this.Name_TextBox.Text == null || this.Name_TextBox.Text.Length == 0)
            {
                error += " \n - Prénom";
            }
            if (this.Surname_TextBox.Text == null || this.Surname_TextBox.Text.Length == 0)
            {
                error += " \n - Nom";
            }

            if (this.Email_TextBox.Text == null || this.Email_TextBox.Text.Length == 0)
            {
                error += " \n - Email";
            }
            if (this.Phone_TextBox.Text == null || this.Phone_TextBox.Text.Length == 0)
            {
                error += " \n - Numéro de téléphone";
            }
            if (this.Mdp_TextBox.Password == null || this.Mdp_TextBox.Password.Length == 0)
            {
                error += " \n - Mot de passe";
            }
            if (this.Mdp_TextBox.Password != this.Mdp_Confirm_TextBox.Password)
            {
                error += " \n - Les deux mots de passe ne sont pas égaux";
            }


            if (error.Length > 0)
            {
                MessageBox.Show("Veuillez remplir les champs suivants : " + error);
            }
            else
            {
                this.infos.Visibility = Visibility.Hidden;
                this.address.Visibility = Visibility.Visible;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}






