using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Fleurs.Utils
{
    public class Utils
    {
        public string connectionString { get; set; }
        
        public Utils()
        {
            this.connectionString = "SERVER=marc.eliqs.dev;DATABASE=Fleurs;UID=marc;PASSWORD=leobddmarximarc;";
        }


    }
}
