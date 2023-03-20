using AspNetCoreHero.ToastNotification.Abstractions;
using DigitalCard.Web.Services;
using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Enums;
using DIgitalCard.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DigitalCard.Web.Controllers
{
    public class CardController : Controller
    { 


    private readonly INotyfService _notyf;
    private readonly IHttpServices _httpServices;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppSettings _appSettings;

      public CardController(INotyfService notyf, IOptions<AppSettings> appSettings,
        IHttpServices httpServices, IHttpContextAccessor httpContextAccessor)
        {
            _notyf = notyf;
            _httpServices = httpServices;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

    
        //[HttpGet]
        //public IActionResult GetCardsForCustomer(int CustomerId)
        //{
        //    string url = $"{_appSettings.ApiBaseUrl}/Cards/GetCustomerCards";
        //    var dataResponse = _httpServices.Get(url);
        //    var result = JsonConvert.DeserializeObject<Response<List<CardDTO>>>(dataResponse.Data);
        //    if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        return View(result.Data);
        //    }
        //    _notyf.Error(result.Message);
        //    return RedirectToAction("GetCardsForCustomer");
        //}


        [HttpGet]
        public IActionResult GetCardsForCustomer()
        {
            string url = $"{_appSettings.ApiBaseUrl}Cards/GetAllCustomerCards";
            var dataResponse = _httpServices.Get(url);
            var result = JsonConvert.DeserializeObject<Response<List<CardDTO>>>(dataResponse.Data);
            if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View(result.Data);
            }
            _notyf.Error(result.Message);
            return RedirectToAction("GetCardsForCustomer");
        }



        


        [HttpGet]
        public IActionResult AddCardForCustomer()
        {
            return View(new AddCardForCustomerDTO());
        }

        [HttpPost]
        public IActionResult AddCardForCustomer(AddCardForCustomerDTO addCardForCustomerDTO)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _notyf.Error(message);
                return View(addCardForCustomerDTO);
            }
            string url = $"{_appSettings.ApiBaseUrl}/Cards/AddCardForCustomer";
            var payload = JsonConvert.SerializeObject(addCardForCustomerDTO);
            var dataResponse = _httpServices.Post(url, payload);
            var addCardResult = JsonConvert.DeserializeObject<Response<dynamic>>(dataResponse.Data);
            if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _notyf.Success(addCardResult.Message);
                return RedirectToAction("AddCardForCustomer");
            }
            _notyf.Error(addCardResult.Message);
            return View(addCardResult);
        }

        [HttpGet]
        public IActionResult RequestUpdateForCardStatus(CardStatus cardStatus,int CardId, int CustomerId)
        {
            RequestCardStatusUpdate requestCardStatusUpdate = new RequestCardStatusUpdate
            {
                CardStatus = cardStatus,
                CardId = CardId,
                CustomerId = CustomerId,
               
            };

            string url = $"{_appSettings.ApiBaseUrl}Cards/RequestCardStatusUpdate";
            var payload = JsonConvert.SerializeObject(requestCardStatusUpdate);
            var dataResponse = _httpServices.Post(url, payload);
            var requestSTatus = JsonConvert.DeserializeObject<Response<dynamic>>(dataResponse.Data);
            if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _notyf.Success(requestSTatus.Message,10);
                return RedirectToAction("ValidateRequest",new { CardId = CardId, CustomerId  = CustomerId });
            }
            _notyf.Error(requestSTatus.Message);
            return RedirectToAction("GetCardsForCustomer");
        }



        [HttpGet]
        public IActionResult ValidateRequest(int CardId,int CustomerId)
        {
            OTPValidation oTPValidation = new OTPValidation
            {
                CardId = 0,
                CustomerId = 0,
                OTP = ""
            };
            return View(oTPValidation);
        }


        [HttpPost]
        public IActionResult ValidateRequest(OTPValidation oTPValidation)
        {
         
            string url = $"{_appSettings.ApiBaseUrl}Cards/ValidateRequest";
            var payload = JsonConvert.SerializeObject(oTPValidation);
            var dataResponse = _httpServices.Post(url, payload);
            var requestSTatus = JsonConvert.DeserializeObject<Response<dynamic>>(dataResponse.Data);
            if (dataResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _notyf.Success(requestSTatus.Message);
                return RedirectToAction("GetCardsForCustomer");
            }
            _notyf.Error(requestSTatus.Message);
            return RedirectToAction("GetCardsForCustomer");
        }
    }
}
