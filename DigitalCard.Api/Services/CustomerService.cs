using AutoMapper;
using DigitalCard.Api.Data;
using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Entities;
using DIgitalCard.Lib.Models;
using DIgitalCard.Lib.Utils;
using Microsoft.Extensions.Options;

namespace DigitalCard.Api.Services
{
    public interface ICustomerService
    {
        Response<int> AddCustomer(AddCustomerDTO addCustomerDTO);

        Response<CustomerDTO> LoginCustomer(LoginCustomerDto loginCustomer);

       
    }
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private EncryptionHelper _encryptionHelper;


        public CustomerService(
      ApplicationDbContext context,
      IMapper mapper,
      IOptions<AppSettings> appSettings,
      IEmailService emailService
      )
        {
            _context = context;

            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _encryptionHelper = new EncryptionHelper(_appSettings.Securitykey, _appSettings.SecurityIv);
        }

        public Response<int> AddCustomer(AddCustomerDTO addCustomerDTO)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(x => x.Email == addCustomerDTO.Email);
            if (existingCustomer != null)
            {
                throw new AppException("Customer Already Exist");
            }


            Customer customerToAdd = new Customer
            {
                DateAdded = DateTime.Now,
                Email = addCustomerDTO.Email,
                FirstName = addCustomerDTO.FirstName,
                LastName = addCustomerDTO.LastName,
            };

            _context.Customers.Add(customerToAdd);
            _context.SaveChanges();

            return new Response<int> { 
            Data = customerToAdd.Id,
            Message = "Sucessful",
            Succeeded = true};
        }

        public Response<CustomerDTO> LoginCustomer(LoginCustomerDto loginCustomer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(x => x.Email == loginCustomer.Email);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException("Customer Does Not Exist");
            }

            return new Response<CustomerDTO> { 
                Succeeded = true,
                Message ="Sucessful",
                Data= _mapper.Map<CustomerDTO>(existingCustomer),
            };
        }
    }
}
