using ECommerce.Business.Abstract;
using ECommerce.Entity.Models;
using ECommerce.WebUI.Extensions;
using ECommerce.WebUI.Identity;
using ECommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IParentCategoryService _parentCategoryService;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(IProductService productService, ICategoryService categoryService,
                               IParentCategoryService parentCategoryService, RoleManager<IdentityRole> roleManager,
                               UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _parentCategoryService = parentCategoryService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(r => r.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel() {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/users");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model,string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        TempData.Put("message", new AlertMessage()
                        {
                            Message = "Kullanıcı Başarıyla Güncellendi!",
                            AlertType = "success"
                        });
                        return Redirect("/admin/users");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }

                    model.SelectedRoles = await _userManager.GetRolesAsync(user);
                    ViewBag.Roles = _roleManager.Roles.Select(r => r.Name);
                    return View(model);
                }

                TempData.Put("message", new AlertMessage()
                {
                    Message = "Kullanıcı Bulunamadı!",
                    AlertType = "warning"
                });
                return Redirect("/admin/users");
            }

            model.SelectedRoles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name);
            return View(model);
        }

            public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/roles/"+model.RoleId);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductList()
        {
            var products = await _productService.GetAllAsync();
            return View(new ProductListViewModel() 
            {
                Products = products
            });
        }

        public async Task<IActionResult> CategoryList()
        {
            var categories = await _categoryService.GetAll();
            return View(new CategoryListViewModel()
            {
                Categories = categories
            });
        }

        public async Task<IActionResult> ProductCreate()
        {
            var model = new ProductModel()
            {
                AllBrands = _productService.GetAllBrands()
            };
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model, int[] categoryIds, string[] attributeNames, string[] attributeValues, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    BrandId = model.Brand.Id,
                    Price = (double)model.Price,
                    Url = model.Url,
                    Description = model.Description
                };

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName); // resmin uzantısını almak için
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (model.ProductAttributes == null)
                    entity.ProductAttributes = new List<ProductAttribute>();
                else
                    entity.ProductAttributes = model.ProductAttributes;

                // yeni özellikleri ekle
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    if (attributeNames[i] != null && attributeValues[i] != null)
                    {
                        entity.ProductAttributes.Add(new ProductAttribute() { Name = attributeNames[i], Value = attributeValues[i] });
                    }
                }

                if(_productService.Create(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Message = $"{entity.Name} isimli ürün eklendi.",
                        AlertType = "success"
                    });

                    return RedirectToAction("ProductList");
                }

                TempData.Put("message", new AlertMessage()
                {
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
                
            }

            model.AllBrands = _productService.GetAllBrands();
            ViewBag.Categories = await _categoryService.GetAll();

            if (model.ProductAttributes == null)
            {
                model.ProductAttributes = new List<ProductAttribute>();
            }
            
            for (int i = 0; i < attributeNames.Length; i++)
            {
                if (attributeNames[i] != null && attributeValues[i] != null)
                {
                    model.ProductAttributes.Add(new ProductAttribute() { Name = attributeNames[i], Value = attributeValues[i] });
                }
            }
            
            return View(model);
        }
        public async Task<IActionResult> CategoryCreate()
        {
            var pCategories = await _parentCategoryService.GetAll();
            var model = new CategoryModel()
            {
                ParentCategories = pCategories
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryCreate(CategoryModel model)
        {

            if (ModelState.IsValid)
            {

                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url,
                    ParentCategoryId = model.ParentCategory.Id
                };

                _categoryService.Create(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Message = $"{entity.Name} isimli kategori eklendi.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }

            model.ParentCategories = await _parentCategoryService.GetAll();
            return View(model);
        }
        public async Task<IActionResult> ProductEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity==null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Brand = entity.Brand,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(pc => pc.Category).ToList(),
                ProductAttributes = entity.ProductAttributes,
                AllBrands = _productService.GetAllBrands()
            };

            ViewBag.Categories = await _categoryService.GetAll();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model,int[] categoryIds,string[] attributeNames,string[] attributeValues,IFormFile file)
        {
            if (ModelState.IsValid) { 
                var entity = _productService.GetByIdWithCategories(model.Id);
                if (entity==null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.BrandId = model.Brand.Id;
                entity.Price = (double)model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName); // resmin uzantısını almak için
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                       await file.CopyToAsync(stream);
                    }
                }

                entity.ProductAttributes.Clear();

                // yeni özellikleri ekle
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    if (attributeNames[i] != null && attributeValues[i]!= null)
                    {
                        model.ProductAttributes.Add(new ProductAttribute() {Name = attributeNames[i],Value = attributeValues[i]});
                    }
                }

                // güncellenen özellikleri ekle
                if (model.ProductAttributes != null)
                {
                    foreach (var attribute in model.ProductAttributes)
                    {
                        entity.ProductAttributes.Add(attribute);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {

                    TempData.Put("message", new AlertMessage()
                    {
                        Message = $"{entity.Name} isimli ürün güncellendi",
                        AlertType = "success"
                    });

                    return RedirectToAction("ProductList");
                }

                TempData.Put("message", new AlertMessage()
                {
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
            }

            // yeni özellikleri ekle
            for (int i = 0; i < attributeNames.Length; i++)
            {
                if (attributeNames[i] != null && attributeValues[i] != null)
                {
                    model.ProductAttributes.Add(new ProductAttribute() { Name = attributeNames[i], Value = attributeValues[i] });
                }
            }
            model.AllBrands = _productService.GetAllBrands();

            if(model.SelectedCategories==null)
            model.SelectedCategories = new List<Category>();

            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }

        public async Task<IActionResult> CategoryEdit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithParentCAndProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }
            var pCategories = await _parentCategoryService.GetAll();
            var model = new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
                ParentCategory = entity.ParentCategory,
                Products = entity.ProductCategories.Select(pc=>pc.Product).ToList(),
                ParentCategories = pCategories
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel model)
        {

            if (ModelState.IsValid)
            {

                var entity = await _categoryService.GetById(model.Id);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.ParentCategory = model.ParentCategory;

                _categoryService.Update(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Message = $"{entity.Name} isimli kategori güncellendi.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }
            var products = _categoryService.GetByIdWithParentCAndProducts(model.Id).ProductCategories;
            model.Products = products.Select(p => p.Product).ToList();

            model.ParentCategories = await _parentCategoryService.GetAll();

            return View(model);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var entity = await _productService.GetByIdAsync(id);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            });

            return RedirectToAction("ProductList");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var entity = await _categoryService.GetById(id);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori silindi.",
                AlertType = "danger"
            });

            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }

    }
}
