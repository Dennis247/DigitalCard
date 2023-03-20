using DigitalCard.Api.Services;
using DIgitalCard.Lib.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalCard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(AddCustomerDTO addCustomerDTO)
        {
            var response = _customerService.AddCustomer(addCustomerDTO);
            return Ok(response);
        }


        [HttpPost("LoginCustomer")]
        public IActionResult LoginCustomer(LoginCustomerDto loginCustomerDto)
        {
            var response = _customerService.LoginCustomer(loginCustomerDto);
            return Ok(response);
        }




    }
}
