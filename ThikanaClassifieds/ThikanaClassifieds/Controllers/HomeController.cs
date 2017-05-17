using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThikanaClassifieds.Models;

namespace ThikanaClassifieds.Controllers
{
    public class HomeController : Controller
    {

        private ThikanaclassifiedsEntities db = new ThikanaclassifiedsEntities();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            List<Object> myModel = new List<object>();
            myModel.Add(db.Classifieds_Category.ToList());
            myModel.Add(db.Classifieds_Items.ToList());
            //myModel.Add(db.Classifieds_Item_Image.FirstOrDefault());               
            return View(myModel);
        }

        [HttpPost]
        public JsonResult AutoSuggest(string searchtext)
        {
            if (searchtext != null && searchtext.Length > 0)
            {
                DateTime DateSearch = DateTime.Today;
                bool flag = false;               
                DateTime NextDate = DateSearch.AddDays(1);
                //var search_data = flag ? DB.News.Where(i => i.News_Title.Contains(searchtext) || i.News_Description.Contains(searchtext) || i.Tags.Contains(searchtext) || (i.Publication_Date >= DateSearch && i.Publication_Date < NextDate)).Take(20) :
                //       DB.News.Where(i => i.News_Title.Contains(searchtext) || i.News_Description.Contains(searchtext) || i.Tags.Contains(searchtext)).Take(20);
                var search_data = flag ? db.Classifieds_Items.Where(i => i.Classifieds_Item_Titel.Contains(searchtext) || i.Classifieds_Item_Description.Contains(searchtext) || i.Classifieds_Item_Location.Contains(searchtext) || i.Classifieds_Item_Price.Contains(searchtext)).Take(20) :
                        db.Classifieds_Items.Where(i => i.Classifieds_Item_Titel.Contains(searchtext) || i.Classifieds_Item_Description.Contains(searchtext) || i.Classifieds_Item_Location.Contains(searchtext) || i.Classifieds_Item_Price.Contains(searchtext)).Take(20);
                return Json(search_data.Select(i => i.Classifieds_Item_Titel).ToArray());
            }
            return Json("");
        }

        public ActionResult searchResult(string searchtext = "")
        {
            if (searchtext != null && searchtext.Trim().Length > 0)
            {
                DateTime DateSearch = DateTime.Today;
                bool flag = false;                
                DateTime NextDate = DateSearch.AddDays(1);
                var search_data = flag ? db.Classifieds_Items.Where(i => i.Classifieds_Item_Titel.Contains(searchtext) || i.Classifieds_Item_Description.Contains(searchtext) || i.Classifieds_Item_Location.Contains(searchtext) || i.Classifieds_Item_Price.Contains(searchtext)).Take(20) :
                        db.Classifieds_Items.Where(i => i.Classifieds_Item_Titel.Contains(searchtext) || i.Classifieds_Item_Description.Contains(searchtext) || i.Classifieds_Item_Location.Contains(searchtext) || i.Classifieds_Item_Price.Contains(searchtext)).Take(20);                
                return View(search_data);
            }
            return View(db.Classifieds_Items.Take(0));
        }

        public ActionResult Adspage(int id, string searchtext)
        {
            
                List<Object> myModel = new List<object>();
                myModel.Add(db.Classifieds_Category.ToList());
                myModel.Add(db.Classifieds_Items.Where(i => i.Classifieds_Category_Id == id).ToList());
                return View(myModel);
            
        }

        public ActionResult ViewAds(int id)
        {
            //List<Object> myModel = new List<object>();
            //myModel.Add(db.Classifieds_Category.ToList());
            //myModel.Add(db.Classifieds_Items.SingleOrDefault(p=>p.Classifieds_Item_Id==id));
            //myModel.Add(db.Classifieds_Item_Image.ToList());
            //return View(myModel);
            

            
            Classifieds_Items CItems = db.Classifieds_Items.SingleOrDefault(p => p.Classifieds_Item_Id == id);
            return View("ViewAds", CItems);
        }
	}
}