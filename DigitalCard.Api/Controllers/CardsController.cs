using DigitalCard.Api.Services;
using DIgitalCard.Lib.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalCard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }


        [HttpPost("AddCardForCustomer")]
        public IActionResult AddCardForCustomer(AddCardForCustomerDTO addCardForCustomerDTO)
        {
            var response = _cardService.AddCardForCustomer(addCardForCustomerDTO);
            return Ok(response);
        }

        [HttpGet("GetCustomerCards")]
        public IActionResult GetCustomerCards(int CustomerId)
        {
            var response = _cardService.GetCustomerCards(CustomerId);
            return Ok(response);
        }

        [HttpGet("GetAllCustomerCards")]
        public IActionResult GetAllCustomerCards()
        {
            var response = _cardService.GetAllCustomerCards();
            return Ok(response);
        }


        [HttpPost("RequestCardStatusUpdate")]
        public IActionResult RequestCardStatusUpdate(RequestCardStatusUpdate requestCardStatusUpdate)
        {
            var response = _cardService.RequestCardStatusUpdate(requestCardStatusUpdate);
            return Ok(response);
        }

        [HttpPost("ValidateRequest")]
        public IActionResult ValidateRequest(OTPValidation oTPValidation)
        {
            var response = _cardService.ValidateRequest(oTPValidation);
            return Ok(response);
        }
    }
}
