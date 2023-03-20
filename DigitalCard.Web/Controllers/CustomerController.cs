using AspNetCoreHero.ToastNotification.Abstractions;
using DigitalCard.Web.Services;
using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DigitalCard.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IHttpServices _httpServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public CustomerController(INotyfService notyf, IOptions<AppSettings> appSettings,
          IHttpServices httpServices, IHttpContextAccessor httpContextAccessor)
        {
            _notyf = notyf;
            _httpServices = httpServices;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet]
        public IActionResult AddCustomers()
        {
            return View(new AddCustomerDTO());
        }

        [HttpPost]
        public IActionResult AddCustomers(AddCustomerDTO addCustomerDTO)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _notyf.Error(message);
                return View(addCustomerDTO);
            }
            string url = $"{_appSettings.ApiBaseUrl}/Customers/AddCustomer";
            var payload = JsonConvert.SerializeObject(addCustomerDTO);
            var dataResponse = _httpServices.Post(url, payload);
            var addCustomerResult = JsonConvert.DeserializeObject<Response<dynamic>>(dataResponse.Data);
            if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _notyf.Success(addCustomerResult.Message);
                return RedirectToAction("AddCustomers");
            }
            _notyf.Error(addCustomerResult.Message);
            return View(addCustomerResult);
        }
    }
}
