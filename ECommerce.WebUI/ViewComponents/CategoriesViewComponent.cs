using ECommerce.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private IParentCategoryService _parentCategoryService;

        public CategoriesViewComponent(IParentCategoryService parentCategoryService)
        {
            this._parentCategoryService = parentCategoryService;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _parentCategoryService.GetAllWithCategories();
            
            /*if (RouteData.Values["action"].ToString().ToLower() == "list")
                ViewBag.SelectedCategory = RouteData?.Values["id"];
            return View(CategoryRepository.Categories);*/
            return View(categories);
        }
    }
}
