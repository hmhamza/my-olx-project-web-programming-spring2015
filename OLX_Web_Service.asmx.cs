using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Web_Finale3.Models;
using Web_Finale3.Controllers;
using System.Web.Script.Serialization;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace Web_Finale3
{
    /// <summary>
    /// Summary description for OLX_Web_Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class OLX_Web_Service : System.Web.Services.WebService
    {

  
        [WebMethod]
        public List<String> Search_For_Me(string Title, string Category, string Location, string Price_From, string Price_To)
        {
            List<AD> result = new List<AD>();
            myOLXController C = new myOLXController();
            result = C.Web_Service_Controller_Func(Title,Category,Location,Price_From , Price_To);

            //var serializer = new XmlSerializer();
            //XmlSerializer serializer1 = new XmlSerializer(typeof(AD));
            var serializer = new JavaScriptSerializer();
            List<String> ads = new List<String>();
            
            for (int i = 0; i < result.Count; i++)
            {
                
                Hashtable hashtable = new Hashtable();

                hashtable.Add("Title", result[i].Title);
                hashtable.Add("Category", result[i].Category);
                hashtable.Add("Location", result[i].Location);
                hashtable.Add("Price", result[i].Price);

              
                ads.Add(serializer.Serialize(hashtable));
            }

            return ads;
        }


        public System.Xml.XmlWriter xmlWriter { get; set; }
    }
}
