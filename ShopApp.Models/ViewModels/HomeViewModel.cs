using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public IEnumerable<Service> ServicesList { get; set; }



    }
}
