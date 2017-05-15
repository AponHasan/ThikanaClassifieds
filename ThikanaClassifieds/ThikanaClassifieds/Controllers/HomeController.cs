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

        public ActionResult Adspage(int id)
        {
            List<Object> myModel = new List<object>();
            myModel.Add(db.Classifieds_Category.ToList());
            myModel.Add(db.Classifieds_Items.Where(i=>i.Classifieds_Category_Id==id).ToList());
            return View(myModel);
        }

        public ActionResult ViewAds(int id)
        {
            //List<Object> myModel = new List<object>();
            //myModel.Add(db.Classifieds_Category.ToList());
            //myModel.Add(db.Classifieds_Items.ToList());
            //myModel.Add(db.Classifieds_Item_Image.ToList());
            //return View(myModel);
            

            
            Classifieds_Items CItems = db.Classifieds_Items.SingleOrDefault(p => p.Classifieds_Item_Id == id);
            return View("ViewAds", CItems);
        }
	}
}