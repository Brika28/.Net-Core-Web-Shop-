using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.DataAccess.Data.Repository.IRepository
{
    public interface IServiceRepository : IRepository<Service>
    {

        void Update(Service service);
    }
}
