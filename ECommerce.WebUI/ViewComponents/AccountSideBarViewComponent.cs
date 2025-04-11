using ECommerce.WebUI.Identity;
using ECommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewComponents
{
    [Authorize]
    public class AccountSideBarViewComponent : ViewComponent
    {
        private UserManager<User> _userManager;
        public AccountSideBarViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
                var model = new UserDetailsModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return View(model);
        }
    }
}
