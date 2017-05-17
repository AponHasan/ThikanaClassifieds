using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThikanaClassifieds.Models;
using ThikanaClassifieds.Utilities;


namespace ThikanaClassifieds.Controllers
{
    public class AdminController : Controller
    {
        private ThikanaclassifiedsEntities db = new ThikanaclassifiedsEntities();
        //
        // GET: /Admin/

        //classified category function start

        public ActionResult Login()
        {
            if(Session["user"]==null)
            {
                RedirectToAction("Login", "Admin");
                
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Name, string Password)
        {
            if("admin".Equals(Name) && "123".Equals(Password))
            {
                Session["user"] = new User() { Login = Name, Name = "Admin" };
                return RedirectToAction("ClassifiedsItemData", "Admin");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login","Admin");
        }

        public ActionResult ClassifiedCategory()
        {
            if (Session["user"] != null)
            {
            return View();
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        public ActionResult ClassifiedCategory(Classifieds_Category category)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    var x = FileUploader.FileUpload(this.ControllerContext).FirstOrDefault();
                    if (x != null && x.Length > 0)
                    {
                        category.Classifieds_Category_Image = x;
                    }
                    db.Classifieds_Category.Add(category);
                    db.SaveChanges();
                }
                return View();
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }
        
        //classified category function end

        //Classified category id selected drop down function start
        private void CategoryIDDropDownList(object selectedCategory = null)
        {
            var categoryIdQuery = from d in db.Classifieds_Category
                                  orderby d.Classifieds_Category_Id
                                  select d;
            ViewBag.CategoryId = new SelectList(categoryIdQuery, "Classifieds_Category_Id", "Classifieds_Category_Name", selectedCategory);
        }
        //Classified category id selected drop down function End


        //list of classified Category
        public ActionResult ClassifiedsCategoryData()
        {
            if (Session["user"] != null)
            {
            return View(db.Classifieds_Category.ToList().OrderByDescending(j => j.Classifieds_Category_Id).ThenByDescending(l => l.Classifieds_Category_Id));
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }
        

        //Delete  classifiedCtegory 
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            if (Session["user"] != null)
            {
            Classifieds_Category CCategory = db.Classifieds_Category.SingleOrDefault(c => c.Classifieds_Category_Id == id);
            return View("DeleteCategory", CCategory);
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }        
      
        [HttpPost, ActionName("DeleteCategory")]
        public ActionResult Delete_Confirm(int id)
        {
            if (Session["user"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Classifieds_Category CCategory = db.Classifieds_Category.Find(id);
                if (CCategory == null)
                {
                    return HttpNotFound();
                }
                if (CCategory != null)
                {
                    db.Classifieds_Category.Remove(CCategory);
                    db.SaveChanges();
                    return RedirectToAction("ClassifiedsCategoryData");
                }
                return View(CCategory);
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
            //Classifieds_Items Citem = db.Classifieds_Items.Include(i => i.Classifieds_Item_Image).Where(i => i.Classifieds_Item_Id == id).Single();

            //db.Classifieds_Items.Remove(Citem);

            //var Cii = db.Classifieds_Item_Image.Where(d => d.Classifieds_Item_Id == id).SingleOrDefault();
            //if (Cii != null)
            //{
            //    Cii.Classifieds_Item_Image1 = null;
            //}

            //db.SaveChanges();
            //return RedirectToAction("ClassifiedsCategoryData");
        }

        //Classified Category Edit
        public ActionResult EditCategory(int id)
        {
            if (Session["user"] != null)
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Classifieds_Category CCategory = db.Classifieds_Category.Find(id);
            if (CCategory == null)
            {
                return HttpNotFound();
            }
            return View(CCategory);
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditCategory(Classifieds_Category CCategory)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(CCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ClassifiedsCategoryData");
                }
                return View(CCategory);
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }



















       


        //Classifide items function start
        public ActionResult ClassifiedItem()
        {
            if (Session["user"] != null)
            {
            CategoryIDDropDownList();
            return View();
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        public ActionResult ClassifiedItem(Classifieds_Items citems)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    var files = this.HttpContext.Request.Files;
                    citems.Date = DateTime.Now;
                    db.Classifieds_Items.Add(citems);
                    var x = FileUploader.FileUpload(this.ControllerContext);
                    foreach (var item in x)
                    {
                        db.Classifieds_Item_Image.Add(new Models.Classifieds_Item_Image() { Classifieds_Items = citems, Classifieds_Item_Image1 = item });
                    }
                    db.SaveChanges();
                }
                CategoryIDDropDownList();
                return View();
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        //Classified Category Item data Retrive
        public ActionResult ClassifiedsItemData()
        {
            if (Session["user"] != null)
            {
            return View(db.Classifieds_Items.ToList().OrderByDescending(j => j.Classifieds_Item_Id).ThenByDescending(l => l.Classifieds_Item_Id));
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        //Classified Category Itemdelete Function
        public ActionResult DeleteCategoryItem(int id)
        {
            if (Session["user"] != null)
            {
            Classifieds_Items CItem = db.Classifieds_Items.SingleOrDefault(ci => ci.Classifieds_Item_Id == id);
            return View("DeleteCategoryItem", CItem);
                }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Classifieds_Items CItem = db.Classifieds_Items.Find(id);
            //if (CItem == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(CItem);
          
        }

        [HttpPost, ActionName("DeleteCategoryItem")]
        public ActionResult Delete_Confirms(int id)
        {
            if (Session["user"] != null)
            {
                var Citem = db.Classifieds_Items.Find(id);
                foreach (var item in Citem.Classifieds_Item_Image.ToList())
                {
                    FileUploader.DeleteFile(this.ControllerContext, item.Classifieds_Item_Image1);
                    db.Classifieds_Item_Image.Remove(item);
                }
                db.Classifieds_Items.Remove(Citem);
                db.SaveChanges();
                return RedirectToAction("ClassifiedsItemData");
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        //Classified Category Items Edit
        public ActionResult EditItems(int id)
        {
            if (Session["user"] != null)
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Classifieds_Items CItem = db.Classifieds_Items.Find(id);
            if (CItem == null)
            {
                return HttpNotFound();
            }
            return View(CItem);
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditItems(Classifieds_Items CItem)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(CItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ClassifiedsItemData");
                }
                return View(CItem);
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
        }
	}
}