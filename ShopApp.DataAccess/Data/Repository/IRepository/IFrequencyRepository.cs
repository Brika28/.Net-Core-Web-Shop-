using Microsoft.AspNetCore.Mvc.Rendering;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository : IRepository<Frequency>
    {
        IEnumerable<SelectListItem> GetFrequencyListForDropDown();

        void Update(Frequency frequency);
    }
}
