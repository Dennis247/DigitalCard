using AutoMapper;
using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Models;
using DIgitalCard.Lib.Utils;
using Microsoft.Extensions.Options;
using  DigitalCard.Api.Data;
using DIgitalCard.Lib.Entities;

namespace DigitalCard.Api.Services
{
    public interface ICardService
    {
        Response<int> AddCardForCustomer(AddCardForCustomerDTO addCardForCustomerDTO);
        Response<List<CardDTO>> GetCustomerCards(int CustomerId);
        Response<int> RequestCardStatusUpdate(RequestCardStatusUpdate requestCardStatusUpdate);
        Response<int> ValidateRequest(OTPValidation oTPValidation);

        Response<List<CardDTO>> GetAllCustomerCards();
    }

    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private EncryptionHelper _encryptionHelper;


        public CardService(
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


        public Response<int> AddCardForCustomer(AddCardForCustomerDTO addCardForCustomerDTO)
        {
            var existingCustomer = _context.Customers.SingleOrDefault(x => x.Email == addCardForCustomerDTO.Email);
            if (existingCustomer == null)
                throw new KeyNotFoundException("Customer Not Found");

            Card card = new Card
            {
                CCV = "789",
                CustomerId = existingCustomer.Id,
                DateCreated = DateTime.UtcNow,
                ExpiryDate = "10/27",
                IsActive = false,
                PAN = generateRandomPan(),
                CardStatus = DIgitalCard.Lib.Enums.CardStatus.ActivateCard,
                CardName = existingCustomer.FirstName + " "+existingCustomer.LastName


            };

            _context.Cards.Add(card);
            _context.SaveChanges();

            return new Response<int>
            {
                Data = card.Id,
                Message = "Card Created Sucessful",
                Succeeded = true,
            };
        }

        

        public Response<List<CardDTO>> GetCustomerCards(int CustomerId)
        {
            List<Card> customerCards = _context.Cards.Where(x => x.CustomerId == CustomerId).ToList();
            foreach (var item in customerCards)
            {
                item.PAN = _encryptionHelper.AESDecrypt(item.PAN);
            }
            return new Response<List<CardDTO>>
            {
                Data = _mapper.Map<List<CardDTO>>(customerCards),
                Message = "sucessful",
                Succeeded = true
            };
        }

        public Response<List<CardDTO>> GetAllCustomerCards()
        {
            List<Card> customerCards = _context.Cards.ToList();
            foreach (var item in customerCards)
            {
                item.PAN = _encryptionHelper.AESDecrypt(item.PAN);
            }
            return new Response<List<CardDTO>>
            {
                Data = _mapper.Map<List<CardDTO>>(customerCards),
                Message = "sucessful",
                Succeeded = true
            };
        }

        public Response<int> RequestCardStatusUpdate(RequestCardStatusUpdate requestCardStatusUpdate)
        {
            Card? existingCard = _context.Cards.SingleOrDefault(x => x.Id == requestCardStatusUpdate.CardId);
            if (existingCard == null)
                throw new KeyNotFoundException("Card does not exist");

            Customer? existingCustomer = _context.Customers.SingleOrDefault(x => x.Id == requestCardStatusUpdate.CustomerId);
            if (existingCustomer == null)
                throw new KeyNotFoundException("Customer does not exist");


            CardStatusRequest cardStatusRequest = new CardStatusRequest
            {
                CardId = requestCardStatusUpdate.CardId,
                CustomerId = existingCustomer.Id,
                CardStatus = requestCardStatusUpdate.CardStatus,
                OTPExpiration = DateTime.Now.AddMinutes(5),
                IsRequestCompleted = false,
                RequestDate = DateTime.Now,
                OTP = generateRandomToken()
            };

          

            sendVerifictionToken(existingCustomer.Email, cardStatusRequest.OTP);
            _context.CardStatusRequests.Add(cardStatusRequest);
            _context.SaveChanges();
            return new Response<int>
            {
                Data = cardStatusRequest.Id,
                Message = "Request Submitted Sucessful, Please check your email for Token",
                Succeeded = true
            };




        }

        public Response<int> ValidateRequest(OTPValidation oTPValidation)
        {
            Card? existingCard = _context.Cards.SingleOrDefault(x => x.Id == oTPValidation.CardId);
            if (existingCard == null)
                throw new KeyNotFoundException("Card does not exist");

            Customer? existingCustomer = _context.Customers.SingleOrDefault(x => x.Id == oTPValidation.CustomerId);
            if (existingCustomer == null)
                throw new KeyNotFoundException("Customer does not exist");

            CardStatusRequest? cardStatusRequest = _context.CardStatusRequests.FirstOrDefault(x => x.OTP == oTPValidation.OTP
           && x.CardId == oTPValidation.CardId && x.CustomerId == oTPValidation.CustomerId && !x.IsRequestCompleted);

            if (cardStatusRequest == null)
                throw new AppException("Request not valid");

            if (DateTime.Now.Subtract(cardStatusRequest.RequestDate).TotalMinutes >= 5)
                throw new AppException("OTP has expired, please try again");

            cardStatusRequest.IsRequestCompleted = true;
            cardStatusRequest.DateCompleted = DateTime.Now;
            existingCard.CardStatus = cardStatusRequest.CardStatus;

            _context.CardStatusRequests.Update(cardStatusRequest);
            _context.Cards.Update(existingCard);
            _context.SaveChanges();

            return new Response<int>
            {
                Data = existingCard.Id,
                Message = "Sucessful",
                Succeeded = true
            };

        }


        private string generateRandomToken()
        {

            var token = new Random().Next(011111, 999999).ToString();

            var tokenIsUnique = !_context.CardStatusRequests.Any(x => x.OTP == token);
            if (!tokenIsUnique)
                return generateRandomToken();
            return token;
        }

        private string generateRandomPan()
        {

            var token = new Random().Next(01111111, 999999999).ToString();
            var encryptedPan = _encryptionHelper.AESEncrypt("5399" + token);

            var tokenIsUnique = !_context.Cards.Any(x => x.PAN == encryptedPan);
            if (!tokenIsUnique)
                return generateRandomToken();
            return encryptedPan;
        }

        private void sendVerifictionToken(string Email, string Token)
        {
            string message = $@"<p>Please enter the token to complete your request, the Token will be valid for 5 minutes:</p>
                            <p><b>{Token}</b></p>";

            _emailService.SendEmailWithSMTP(new Message
            {
                To = new List<string> { Email },
                Subject = "Digital Card - Verification",
                HtmlContent = $@"<h4>Verfification Token</h4>
                        {message}"
            });

            //_emailService.SendEmailWithSendGrid(new Message
            //{
            //    To = new List<string> { Email },
            //    Subject = "Digital Card - Verification",
            //    HtmlContent = $@"<h4>Verfification Token</h4>
            //            {message}"
            //});
        }
    }
}
