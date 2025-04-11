using ECommerce.Business.Abstract;
using ECommerce.WebUI.EmailServices;
using ECommerce.WebUI.Extensions;
using ECommerce.WebUI.Identity;
using ECommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken] //Tüm postlarda tokeni kontrol etmek için
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        private IOrderService _orderService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService, IOrderService orderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
            _orderService = orderService;
        }
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken] Formdaki tokeni kontrol etmek için
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu E-postaya ait bir kullanıcı bulunamadı.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen E-postanıza gelen link ile hesabınızı onaylayın.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");//ReturnUrl null ise anasayfaya git
            }

            ModelState.AddModelError("", "Yanlış Parola girişi yapıldı.");

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Email= model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "customer");
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account",new { 
                    userId=user.Id,
                    token=code
                });
                //cart objesini oluştur
                _cartService.InitializeCart(user.Id);
                //email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href ='http://localhost:28509{url}'>tıklayınız.</a>");
                return RedirectToAction("Login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                Message = "Oturum kapatıldı.",
                AlertType = "warning"
            });
            return Redirect("~/");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId==null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Geçersiz Token.",
                    AlertType = "danger"
                });

                return Redirect("/account/login");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Message = "Hesabınız Onaylandı.",
                        AlertType = "success"
                    });
                    
                    return Redirect("/account/login");
                }
            }

            TempData.Put("message", new AlertMessage()
            {
                Message = "Hesabınız onaylanmadı.",
                AlertType = "danger"
            });
            return Redirect("/account/login");
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "E-posta bulunamadı.",
                    AlertType = "danger"
                });
                
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Kullanıcı bulunamadı.",
                    AlertType = "danger"
                });
               
                return View();
            }

            //generate token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword","Account", new
            {
                userId = user.Id,
                token = token
            });
            //email
            await _emailSender.SendEmailAsync(Email, "Parolanızı yenileyiniz.", $"Lütfen parolanızı yenilemek için linke <a href ='http://localhost:28509{url}'>tıklayınız.</a>");
            
            TempData.Put("message", new AlertMessage()
            {
                Message = "E-postanıza gönderilen linkten parolanızı yenileyebilirsiniz.",
                AlertType = "warning"
            });
            
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string userId,string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Geçersiz bağlantı.",
                    AlertType = "warning"
                });
                
                return RedirectToAction("Index","Home");
            }

            var model = new ResetPasswordModel { Token = token };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Kullanıcı bulunamadı.",
                    AlertType = "warning"
                });
                
                return RedirectToAction("Index","Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Şifreniz başarıyla değiştirilmiştir.Yeni şifrenizle giriş yapabilirsiniz.",
                    AlertType = "success"
                });

                return RedirectToAction("Login","Account");
            }

            TempData.Put("message", new AlertMessage()
            {
                Message = "Şifreniz değiştirilememiştir. Geçersiz bağlantı!",
                AlertType = "danger"
            });
            
            return View(model);
        }
        
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();

                orderModel.Id = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Address = order.Address;
                orderModel.City = order.City;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentType = order.PaymentType;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    Id = i.Id,
                    Name = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImageUrl

                }).ToList();

                orderListModel.Add(orderModel);

            }

            return View("Orders", orderListModel);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}