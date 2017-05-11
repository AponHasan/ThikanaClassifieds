using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThikanaClassifieds.Models;

namespace ThikanaClassifieds.ViewModel
{
    public class ItemAddViewModel 
    {
        public IEnumerable<ThikanaClassifieds.Models.Classifieds_Items> classifiedsItem { get; set; }
        public IEnumerable<ThikanaClassifieds.Models.Classifieds_Category> clasifiedsCategory { get; set; }
    }
}