using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
           
            return View(products);
        }
        public IActionResult UpdateInsert(int? id)
        {

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            { Text = u.Name, Value = u.CategoryId.ToString() }
               );

            ProductVM productVM = new() { CategoryList = CategoryList, Product = new Product()};

            if(id==0 || id==null)
            {
                //Insert
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
                return View(productVM);

            }

            
        }
        [HttpPost]
        public IActionResult UpdateInsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if(file!=null)
                {
                    string fileName=Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product\");

                    if(!String.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath= Path.Combine(wwwRootPath , productVM.Product.ImageUrl.TrimStart('\\'));
                        
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream=new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    { 
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;

                }

                if(productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                { 
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }
               
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            }
            else {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                { Text = u.Name, Value = u.CategoryId.ToString() }
              );

                return View(productVM); 
            }
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new { data= products });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=> u.Id == id);
            if(productToBeDeleted == null) 
            { return Json(new { success = false, message = "Error while deleting" }); }

            if (!String.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                //delete the old image
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted the product Successfully" });
        }
        #endregion

    }

}
