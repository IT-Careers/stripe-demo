using DemoPayment.App.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace DemoPayment.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ChargeService chargeService;

        public ProductController(ChargeService chargeService)
        {
            this.chargeService = chargeService;

            if (models.Count == 0)
            {
                models.Add("Coca Cola 2l.", new PaymentModel
                {
                    ProductName = "Coca Cola 2l.",
                    Amount = 2.33M,
                    Company = "Idea",
                    Description = "Coca Cola 2l. for 2.33$",
                    Label = "Pay 2.33$"
                });

                models.Add("Nutella", new PaymentModel
                {
                    ProductName = "Nutella",
                    Amount = 5.50M,
                    Company = "Idea",
                    Description = "Nutella for 5.50$",
                    Label = "Pay 5.50$"
                });

                models.Add("Krastavica", new PaymentModel
                {
                    ProductName = "Krastavica",
                    Amount = 50,
                    Company = "Idea",
                    Description = "Krastavica for 50.00$",
                    Label = "Pay 50.00$"
                });
            }
        }

        private static List<string> products = new List<string>() { "Coca Cola 2l.", "Krastavica", "Nutella" };

        private static Dictionary<string, PaymentModel> models = new Dictionary<string, PaymentModel>();


        public IActionResult Index()
        {
            return View(products);
        }

        [HttpGet("/Product/Order/{id}")]
        public IActionResult Order(string id)
        {
            return View(models[id]);
        }

        [HttpPost("/Product/Order/{id}")]
        public IActionResult Order(string id, string stripeToken, string stripeEmail)
        {
            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Product", id);
            Metadata.Add("Quantity", "1");

            var options = new ChargeCreateOptions
            {
                Amount = (long)(models[id].Amount * 100),
                Currency = "USD",
                Description = models[id].Description,
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };

            var charge = this.chargeService.Create(options);

            return Redirect("/");
        }
    }
}
