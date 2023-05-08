namespace Fleurs.Utils
{
    public class Utils
    {
        public string connectionString { get; set; }
        
        public Utils()
        {
            this.connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=leobddmarximarc;";
            //this.connectionString = "SERVER=localhost;PORT=3306;DATABASE=BelleFleur;UID=root;PASSWORD=root;";
        }


    }
}
