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

namespace Fleurs.Windows
{
    /// <summary>
    /// Logique d'interaction pour PersonnaliseeOuStandard.xaml
    /// </summary>
    public partial class PersonnaliseeOuStandard : Page
    {
        public PersonnaliseeOuStandard()
        {
            InitializeComponent();
        }

        private void B_Standard_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Standard choix_Bouquet_Standard = new Choix_Bouquet_Standard();
            this.Content = choix_Bouquet_Standard;
        }

        private void B_P_Button_Click(object sender, RoutedEventArgs e)
        {
            Choix_Bouquet_Personnalisee choix_Bouquet_Personnalisee = new Choix_Bouquet_Personnalisee();
            this.Content = choix_Bouquet_Personnalisee;
        }
    }
}
