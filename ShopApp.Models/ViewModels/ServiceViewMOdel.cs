using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Models.ViewModels
{
    public class ServiceViewMOdel
    {
        public Service Service { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> FrequencyList { get; set; }
    }
}
