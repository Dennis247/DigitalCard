using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Models;

namespace DigitalCard.Api.Services
{
    public interface ICustomerService
    {
        Response<int> AddCustomer(AddCustomerDTO addCustomerDTO);

        Response<int> LoginCustomer(LoginCustomerDto loginCustomer);
    }
    public class CustomerService : ICustomerService
    {
        public Response<int> AddCustomer(AddCustomerDTO addCustomerDTO)
        {
            throw new NotImplementedException();
        }

        public Response<int> LoginCustomer(LoginCustomerDto loginCustomer)
        {
            throw new NotImplementedException();
        }
    }
}
