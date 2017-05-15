using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThikanaClassifieds.Models;
namespace ThikanaClassifieds.ViewModel
{
    public class Page1ViewModel
    {
        public int itemCount { get; set; }
        public IEnumerable<Classifieds_Category> CCategory { get; set; }
        public IEnumerable<Classifieds_Items> CItems { get; set; }
        public IEnumerable<Classifieds_Item_Image> CItemsImg { get; set; }
    }
}