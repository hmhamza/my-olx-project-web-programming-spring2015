using MailChimp;
using MailChimp.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Mail;
using System.Web.Mvc;
using Web_Finale3.Models;

using PagedList;
using System.IO;
using Facebook;

using System.Security.Cryptography;
using System.Text;

namespace Web_Finale3.Controllers
{
    public class myOLXController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Home");
        }

        public ActionResult Home()
        {
            if (Session["Email"] == null)
                ViewBag.Flag = false;
            else
                ViewBag.Flag = true;
            return View();
        }

        public ActionResult Submit_Ad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit_Ad_Post(FormCollection form, HttpPostedFileBase photo1, HttpPostedFileBase photo2, HttpPostedFileBase photo3)
        {
            OLX_Context cont = new OLX_Context();

            AD ad = new AD();
            TryUpdateModel(ad);

            if (form["Category"] == "1")
            {
                ad.Category = "Car";
                ad.Param1 = form["Car_Brand"];
                ad.Param2 = form["Mileage"];

            }
            else if (form["Category"] == "2")
            {
                ad.Category = "Fashion";
                ad.Param1 = form["Fashion_Categories"];
                ad.Param2 = form["Gender_Categories"];
            }

            else if (form["Category"] == "3")
            {
                ad.Category = "Furniture";
                ad.Param1 = form["Furniture_Categories"];
            }
            else if (form["Category"] == "4")
            {
                ad.Category = "House";
                ad.Param1 = form["Rooms"];
                ad.Param2 = form["Area"];
            }
            else if (form["Category"] == "5")
            {
                ad.Category = "Mobile";
                ad.Param1 = form["Mobile_Brand"];
            }

            if (ModelState.IsValid)
            {
                cont.ADS.Add(ad);
                if (cont.Ratings.Find(form["Email"]) == null)
                {
                    Rating r = new Rating();
                    r.Email = form["Email"];
                    r.RatedBy = 0;
                    r.Total = 0;
                    cont.Ratings.Add(r);
                }

                cont.SaveChanges();

                string path;
                Image img = new Image();

                path = @"D:\Dropbox\Dropbox\OK\Web_Finale3\Images\";
                if (photo1 != null)
                {
                    photo1.SaveAs(path + photo1.FileName);
                    path = @"\Images\";
                    img.Path = path + photo1.FileName;
                    img.AD_ID = ad.ID;
                    cont.Images.Add(img);
                }

                cont.SaveChanges();

                path = @"D:\Dropbox\Dropbox\OK\Web_Finale3\Images\";
                if (photo2 != null)
                {
                    photo2.SaveAs(path + photo2.FileName);
                    path = @"\Images\";
                    img.Path = path + photo2.FileName;
                    img.AD_ID = ad.ID;
                    cont.Images.Add(img);
                }

                cont.SaveChanges();

                path = @"D:\Dropbox\Dropbox\OK\Web_Finale3\Images\";
                if (photo3 != null)
                {
                    photo3.SaveAs(path + photo3.FileName);
                    path = @"\Images\";
                    img.Path = path + photo3.FileName;
                    img.AD_ID = ad.ID;
                    cont.Images.Add(img);
                }

                cont.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        public ActionResult Login()
        {
            if (Session["Email"] == null)
            {
                ViewBag.Error = "";
                return View();
            }
            else
            {
                return RedirectToAction("User_ADS");

            }
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            string email = form["Email"];

            OLX_Context cont = new OLX_Context();

            User u = cont.Users.SingleOrDefault(x => x.Email == email);

            if (u == null)
            {
                ViewBag.Error = "Email not found!!!";

                //return RedirectToAction("Login");
                return View();

            }
            else if (u.Password != form["Password"])
            {
                ViewBag.Error = "Incorrect Password!!";
                //return RedirectToAction("Login");
                return View();

            }
            else
            {

                Session["Email"] = form["Email"];
                return RedirectToAction("User_ADS");
            }
        }

        public ActionResult SignUp()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection form)
        {

            User u = new User();
            TryUpdateModel(u);

            string email = u.Email;
            OLX_Context cont = new OLX_Context();
            User u1 = cont.Users.SingleOrDefault(x => x.Email == email);
            if (u1 != null)
            {
                ViewBag.Error = "Email already Exists!!!";
                return View();

            }

            if (ModelState.IsValid)
            {
                u.FB_ID = "X";
                cont.Users.Add(u);
                cont.SaveChanges();

            }
            //JavaScriptResult jr=JavaScript("Alert.render('Hello');");

            return RedirectToAction("Login");
        }


        //public ActionResult Search_Ad(FormCollection form)
        //{
        //    OLX_Context cont = new OLX_Context();

        //    string Category = form["Categories"];
        //    if (Category == "All")
        //        Category = "";

        //    string Location = form["Location"];
        //    if (Location == "All")
        //        Location = "";

        //    string Title=form["Input_Title"];


        //    int Price_Form = Convert.ToInt32(form["Price_From"]);
        //    int Price_To = Convert.ToInt32(form["Price_To"]);
        //    if (Price_To == 0)
        //        Price_To = 1000000;


        //    string Param1;
        //    string Param2;

        //    List<AD> ads = ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();
        //    if (Category == "Car")
        //    {
        //        Param1 = form["Param1"];
        //        int m1 = Convert.ToInt32(form["Mileage_From"]);
        //        int m2 = Convert.ToInt32(form["Mileage_To"]);
        //        if (m2 == 0)
        //            m2 = 25000;


        //        ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();

        //        for (int i = 0; i < ads.Count; i++)
        //            if (Convert.ToInt32(ads.ElementAt(i).Param2) < m1 || Convert.ToInt32(ads.ElementAt(i).Param2) > m2)
        //            {
        //                ads.Remove(ads.ElementAt(i));
        //                i--;
        //            }
        //    }

        //    else if (Category == "Fashion")
        //    {
        //        Param1 = form["Param1"];
        //        if (Param1 == "All")
        //            Param1 = "";

        //        Param2 = form["Param2"];
        //        if (Param2 == "All")
        //            ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();
        //        else
        //            ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1) && x.Param2 == Param2).ToList();
        //    }
        //    else if (Category == "Furniture")
        //    {
        //        Param1 = form["Param1"];
        //        if (Param1 == "All")
        //            Param1 = "";
        //        ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();
        //    }
        //    else if (Category == "House")
        //    {

        //        int r = Convert.ToInt32(form["Rooms"]); 

        //        int a1 = Convert.ToInt32(form["Area_From"]);
        //        int a2 = Convert.ToInt32(form["Area_To"]);
        //        if (a2 == 0)
        //            a2 = 500;


        //        ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();

        //        for (int i = 0; i < ads.Count; i++)
        //            if ((r>0 && r < 3 && Convert.ToInt32(ads.ElementAt(i).Param1) != r) || (r >= 3 && Convert.ToInt32(ads.ElementAt(i).Param1) < r) || Convert.ToInt32(ads.ElementAt(i).Param2) < a1 || Convert.ToInt32(ads.ElementAt(i).Param2) > a2)
        //            {
        //                ads.Remove(ads.ElementAt(i));
        //                i--;
        //            }
        //    }
        //    else if (Category == "Mobile")
        //    {
        //        Param1 = form["Param1"];

        //        ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();


        //    }
        //    
        //    ViewBag.ADS = ads;
        //    return View();

        //    

        //   }


        public PartialViewResult Search_Ad(int? page, string Location = "", string Categories = "", string Input_Title = "", int Price_From = 0, int Price_To = 0, string Param1 = "", string Param2 = "", string Area_From = "", string Area_To = "", string Mileage_From = "", string Mileage_To = "")
        {
            OLX_Context cont = new OLX_Context();

            string Category = Categories;
            if (Category == "All")
                Category = "";

            //string Location = form["Location"];
            if (Location == "All")
                Location = "";

            string Title = Input_Title;


            int Price_Form = Price_From;
            //int Price_To = Convert.ToInt32(form["Price_To"]);
            if (Price_To == 0)
                Price_To = 1000000;


            //string Param1;
            //string Param2;

            List<AD> ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();
            if (Category == "Car")
            {
                //Param1 = form["Param1"];
                int m1 = Convert.ToInt32(Mileage_From);
                int m2 = Convert.ToInt32(Mileage_To);
                if (m2 == 0)
                    m2 = 25000;


                ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();

                for (int i = 0; i < ads.Count; i++)
                    if (Convert.ToInt32(ads.ElementAt(i).Param2) < m1 || Convert.ToInt32(ads.ElementAt(i).Param2) > m2)
                    {
                        ads.Remove(ads.ElementAt(i));
                        i--;
                    }
            }

            else if (Category == "Fashion")
            {
                //Param1 = form["Param1"];
                if (Param1 == "All")
                    Param1 = "";

                //Param2 = form["Param2"];
                if (Param2 == "All")
                    ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();
                else
                    ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1) && x.Param2 == Param2).ToList();
            }
            else if (Category == "Furniture")
            {
                // Param1 = form["Param1"];
                if (Param1 == "All")
                    Param1 = "";
                ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To && x.Param1.Contains(Param1)).ToList();
            }
            else if (Category == "House")
            {

                int r = Convert.ToInt32(Param1);

                int a1 = Convert.ToInt32(Area_From);
                int a2 = Convert.ToInt32(Area_To);
                if (a2 == 0)
                    a2 = 500;


                ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();

                for (int i = 0; i < ads.Count; i++)
                    if ((r > 0 && r < 3 && Convert.ToInt32(ads.ElementAt(i).Param1) != r) || (r >= 3 && Convert.ToInt32(ads.ElementAt(i).Param1) < r) || Convert.ToInt32(ads.ElementAt(i).Param2) < a1 || Convert.ToInt32(ads.ElementAt(i).Param2) > a2)
                    {
                        ads.Remove(ads.ElementAt(i));
                        i--;
                    }
            }
            else if (Category == "Mobile")
            {
                //Param1 = form["Param1"];

                ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= Price_Form && x.Price <= Price_To).ToList();


            }
            //ViewBag.ADS = ads;
            //return View(ads);

            int pageSize = 150;
            int pageNumber = (page ?? 1);

            return PartialView("Search_Ad", ads.ToPagedList(pageNumber, pageSize));

        }

        public ViewResult AD_Details(int id, bool done = false)
        {
            OLX_Context cont = new OLX_Context();
            AD ad = cont.ADS.Single(a => a.ID == id);
            Rating r = cont.Ratings.Single(ra => ra.Email == ad.Email);

            ViewBag.ad = ad;

            ViewBag.Flag = true;

            if (Session["Email"] != null && ad.Email == Session["Email"].ToString())
                ViewBag.Flag = false;

            if (r.RatedBy == 0)
                ViewBag.Rating = "--";
            else
            {
                double d = Convert.ToDouble(r.Total) / Convert.ToDouble(r.RatedBy);
                string s = Convert.ToString(d);
                if (s.Length > 3)
                    s = s.Substring(0, 3);
                ViewBag.Rating = s;
            }
            ViewBag.RatedBy = r.RatedBy;

            List<Image> imgs = cont.Images.Where(i => i.AD_ID == ad.ID).ToList();
            ViewBag.IMGS = imgs;


            ViewBag.DONE = done;
            return View();
        }

        public ActionResult Send_Email(string Toemail, FormCollection form)
        {

            try
            {
                OLX_Context cont = new OLX_Context();
                Message m = new Message();
                m.MessageBody = form["Message_Body"];
                m.From = form["Sender_Email"];
                m.To = Toemail;
                cont.Messages.Add(m);
                cont.SaveChanges();

                var api = new MandrillApi("UfOYDeGFVADrZwuEoVdg3g");
                var message = new Mandrill.Messages.Message();
                message.Subject = "Dummy OLX Message";
                message.Text = form["Message_Body"];
                message.Text += "\nFROM: ";
                message.Text += form["Sender_Email"];
                message.FromEmail = "haza4013@gmail.com";
                message.FromName = "HaZa_OLX";
                message.To = new[] { new Mandrill.Messages.Recipient(Toemail, "test") };

                api.Send(message);
                Response.Write("Email Sent");
            }
            catch (Exception ex)
            {
                Response.Write("Could not send the e-mail - error: " + ex.Message);
            }

            return null;
        }

        public ActionResult Rate_User(string UserEmail, int ID, FormCollection form)
        {
            OLX_Context cont = new OLX_Context();
            Rating r = cont.Ratings.Find(UserEmail);
            int rating = Convert.ToInt32(form["Rating_Value"]);
            r.Total += rating;
            r.RatedBy++;
            cont.SaveChanges();

            return RedirectToAction("AD_Details", new { id = ID, done = true });
        }

        public ActionResult User_ADS()
        {

            if (Session["Email"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                OLX_Context cont = new OLX_Context();
                string email = Session["Email"].ToString();
                List<AD> ads = cont.ADS.Where(x => x.Email == email).ToList();
                ViewBag.ADS = ads;
                ViewBag.Name = cont.Users.Single(x => x.Email == email).Name;
                return View();
            }
        }

        public ActionResult User_Messages()
        {
            if (Session["Email"] == null)
                return RedirectToAction("Login");
            else
            {
                OLX_Context cont = new OLX_Context();
                string email = Session["Email"].ToString();
                List<Message> msgs = cont.Messages.Where(x => x.To == email || x.From == email).ToList();
                ViewBag.Messages = msgs;
                ViewBag.Name = cont.Users.Single(x => x.Email == email).Name;
                ViewBag.Email = email;
                return View();
            }


        }

        public ActionResult User_Settings()
        {
            if (Session["Email"] == null)
                return RedirectToAction("Login");
            else
            {
                OLX_Context cont = new OLX_Context();
                string email = Session["Email"].ToString();
                ViewBag.Name = cont.Users.Single(x => x.Email == email).Name;
                return View();
            }
        }

        public ActionResult User_Facebook()
        {
            if (Session["Email"] == null)
                return RedirectToAction("Login");
            else
            {
                OLX_Context cont = new OLX_Context();
                string email = Session["Email"].ToString();

                ViewBag.Name = cont.Users.Single(x => x.Email == email).Name;

                //int fCount = 2;

                //string[][] friendsInfo = new string[20][];
                //for (int i = 0; i < 20; i++)
                //{
                //    friendsInfo[i] = new string[1];
                //}


                //friendsInfo[0][0] = "Musa";
                //friendsInfo[1][0] = "Waleed";

                int fCount = (int)Session["fCount"];                        //It will work when you login from Facebook
                string[][] friendsInfo = (string[][])Session["FB_Friends"];

                List<AD>[] friendsAds = new List<AD>[fCount];

                for (int i = 0; i < fCount; i++)
                {
                    string fID = friendsInfo[i][0];
                    User f = cont.Users.SingleOrDefault(x => x.FB_ID == fID);
                    friendsAds[i] = new List<AD>();
                    if (f != null)
                    {
                        List<AD> ads = cont.ADS.Where(x => x.Email == f.Email).ToList();
                        
                        friendsAds[i] = ads;
                        
                    }
                    
                }

                ViewBag.fCount = fCount;
                ViewBag.FriendsInfo = friendsInfo;
                ViewBag.Friends_ADS = friendsAds;
                return View();
            }


        }

        public ActionResult Change_Contact_Details(FormCollection form)
        {
            OLX_Context cont = new OLX_Context();
            string email = Session["Email"].ToString();
            User u = cont.Users.Single(x => x.Email == email);

            u.Location = form["Location"];

            if (form["Name"] != "")
                u.Name = form["Name"];
            if (form["Phone"] != "")
                u.Phone = form["Phone"];

            cont.SaveChanges();

            return RedirectToAction("User_Settings");

        }

        public ActionResult Change_Password(FormCollection form)
        {
            OLX_Context cont = new OLX_Context();
            string email = Session["Email"].ToString();
            User u = cont.Users.Single(x => x.Email == email);

            u.Password = form["Password"];

            cont.SaveChanges();

            return RedirectToAction("User_Settings");

        }

        public ActionResult Change_Email(FormCollection form)
        {
            OLX_Context cont = new OLX_Context();
            string email = Session["Email"].ToString();
            User u = cont.Users.Single(x => x.Email == email);

            u.Email = form["Email"];
            Session["Email"] = u.Email;
            cont.SaveChanges();

            return RedirectToAction("User_Settings");

        }

        public ActionResult LogOut(FormCollection form)
        {
            Session.RemoveAll();
            return RedirectToAction("Home");

        }

        public List<AD> Web_Service_Controller_Func(string Title, string Category, string Location, string Price_From, string Price_To)
        {
            OLX_Context cont = new OLX_Context();

            if (Category == "All")
                Category = "";

            if (Location == "All")
                Location = "";

            int P_From = Convert.ToInt32(Price_From);
            int P_To = Convert.ToInt32(Price_To);
            if (P_To == 0)
                P_To = 1000000;

            List<AD> ads = ads = cont.ADS.Where(x => x.Category.Contains(Category) && x.Location.Contains(Location) && x.Title.Contains(Title) && x.Price >= P_From && x.Price <= P_To).ToList();

            return ads;
        }

        public ActionResult Visit_Facebook()
        {

            string[][] friendsInfo = new string[20][];
            for (int i = 0; i < 20; i++)
                friendsInfo[i] = new string[2];

            string[] myInfo = new string[10];

            int fCount = 0;
            if (facebookConnect(ref friendsInfo, ref myInfo, ref fCount))
            {
                OLX_Context cont = new OLX_Context();
                var Femail = myInfo[2];
                User u = cont.Users.SingleOrDefault(x => x.Email == Femail);

                if (u == null)
                {                          //0:Name    1:ID    2:Email 

                    User newUser = new User();

                    newUser.Name = myInfo[0];
                    newUser.FB_ID = myInfo[1];
                    newUser.Email = myInfo[2];
                    newUser.Password = "123";


                    cont.Users.Add(newUser);
                    cont.SaveChanges();

                    Session["Email"] = newUser.Email;
                    Session["FB_Friends"] = friendsInfo;
                    Session["fCount"] = fCount;
                    return RedirectToAction("User_Facebook");
                }
                else
                {
                    if (u.FB_ID == "X")
                    {
                        u.FB_ID = myInfo[1];
                        cont.SaveChanges();
                    }
                    Session["Email"] = u.Email;
                    Session["FB_Friends"] = friendsInfo;
                    Session["fCount"] = fCount;
                    return RedirectToAction("User_Facebook");
                }
            }
            return null;

        }

        private bool facebookConnect(ref string[][] friendsInfo, ref string[] myInfo, ref int fCount)
        {

            //olx db = new MYOLXEntities1();
            string app_id = "1663112157245560";
            string app_secret = "c1330e2ad062494982eddaaab0c784d3";
            string scope = "email,user_friends";
            string para = "";
            string Name = "";
            if (Request["code"] == null)
            {
                Response.Redirect(string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", app_id, Request.Url.AbsoluteUri, scope));
                return false;
            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();
                string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}", app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();
                    foreach (string token in vals.Split('&'))
                    {
                        tokens.Add(token.Substring(0, token.IndexOf("=")), token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                    }
                }



                string access = tokens["access_token"];
                var fb = new FacebookClient(access);
                dynamic jsonresult = fb.Get("me?fields=friends,name,email");





                //  return jsonresult;
                //  dynamic jsonresult = fb.Get("/me/friends");
                //     var parse = JObject.Parse(jsonresult);

                //   dynamic me = 

                //PostCrate b=new PostCrate();

                //Post c=new Post();

                //int i = 0;

                myInfo[0] = jsonresult.name;
                myInfo[1] = jsonresult.id;
                myInfo[2] = jsonresult.email;

                string q = myInfo[2];


                //   persondata= db.people.Where(x => x.email ==q);


                int i = 0;
                fCount = 0;
                foreach (dynamic friend in jsonresult.friends.data)
                {
                    fCount++;
                    friendsInfo[i][0] = friend.id;
                    friendsInfo[i][1] = friend.name;
                    //id[i] = friend.id;
                    //name[i] = friend.name;
                    i++;
                }


                //  para = jsonresult["email"];s

                //var friends = jsonresult.friends;
                //     List<JsonArray> ID = null;
                //    ListItem item;
                // var friends = jsonresult.friends;



                //foreach (var friend in (JsonArray)friends["data"])
                //{
                //    //item = new ListItem((string)(((JsonObject)friend)["name"]), (string)(((JsonObject)friend)["id"]));
                //   // ListBox1.Items.Add(item);
                //}

                //foreach (string friend in (JsonArray)jsonresult.friends.data) 
                //{
                //   // Name = friend.id;
                //    ID.Add(friend);

                //}
                return true;
            }

            ;

        }

        public ActionResult Main_Search_Ad()
        {
            return View();
        }
    }
}