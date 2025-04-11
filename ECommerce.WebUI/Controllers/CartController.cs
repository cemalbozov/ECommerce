using ECommerce.Business.Abstract;
using ECommerce.Entity.Models;
using ECommerce.WebUI.Extensions;
using ECommerce.WebUI.Identity;
using ECommerce.WebUI.ViewModels;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<User> _userManager;
        public CartController(ICartService cartService, IOrderService orderService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var model = new CartModel()
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    Id=i.Id,
                    ProductId = i.ProductId,
                    Name=i.Product.Name,
                    Brand = i.Product.Brand,
                    Discount = i.Product.Discount,
                    Price = i.Product.Price,
                    DiscPrice=i.Product.DiscPrice,
                    ImageUrl=i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId,productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Brand = i.Product.Brand,
                    Discount = i.Product.Discount,
                    Price = i.Product.Price,
                    DiscPrice = i.Product.DiscPrice,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };
            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    Id = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Brand = i.Product.Brand,
                        Discount = i.Product.Discount,
                        Price = i.Product.Price,
                        DiscPrice = i.Product.DiscPrice,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity

                    }).ToList()
                };

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(model.CartModel.Id);
                    return View("Success");
                }
                else
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Message = $"{payment.ErrorMessage}",
                        AlertType = "danger"
                    });
                }
            }
            return View(model);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.completed;
            order.PaymentType = EnumPaymentType.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.City = model.City;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.OrderItems = new List<Entity.Models.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new Entity.OrderItem()
                {
                    Price = item.Discount?(double)item.DiscPrice:(double)item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-LYYf4Bvz0vQX79jQQURUYM8pYlOcuR2A";
            options.SecretKey = "5WlByfzII2cvX3joSKMw4iyQZkztjBy2";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111,999999999).ToString();
            request.Price = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            /*paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";*/

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = model.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    basketItem = new BasketItem();
                    basketItem.Id = item.Id.ToString();
                    basketItem.Name = item.Name;
                    basketItem.Category1 = "Telefon";
                    basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                    basketItem.Price = item.Discount ? item.DiscPrice.ToString() : item.Price.ToString();

                    basketItems.Add(basketItem);
                }
            }

            request.BasketItems = basketItems;

            return Payment.Create(request, options);
        }
    }
}
