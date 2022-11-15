using DIgitalCard.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIgitalCard.Lib.DTO
{
    public class AddCardForCustomerDTO
    {
        [Required]
        public int CustomerId { get; set; }
    }


    public class RequestCardStatusUpdate
    {
        public int CardId { get; set; }
        public int CustomerId { get; set; }
        public CardStatus CardStatus { get; set; }
    }


    public class OTPValidation
    {
        public int CustomerId { get; set; }
        public int CardId { get; set; }
        public string OTP { get; set; }
    }

    public class CardDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PAN { get; set; }
        public string ExpiryDate { get; set; }
        public string CCV { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
