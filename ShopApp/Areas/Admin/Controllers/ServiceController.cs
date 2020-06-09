using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ShopApp.DataAccess.Data.Repository.IRepository;
using ShopApp.Models;
using ShopApp.Models.ViewModels;

namespace ShopApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceViewMOdel serVM { get; set; }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
             serVM = new ServiceViewMOdel()
            {
                Service = new Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown(),
            };
            if (id != null)
            {
                serVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(serVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (serVM.Service.Id == 0)
                {
                    //New service is created 
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    serVM.Service.ImgUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(serVM.Service);
                }
                else
                {
                    //Edit or update
                    var serviceFromDb = _unitOfWork.Service.Get(serVM.Service.Id);

                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var upload = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImgUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        serVM.Service.ImgUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        serVM.Service.ImgUrl = serviceFromDb.ImgUrl;
                    }

                    _unitOfWork.Service.Update(serVM.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                serVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                serVM.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(serVM);
            }
        }


        #region API Calls

        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;

            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImgUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete is succsesful!" });
        }

        #endregion
    }
}