using DIgitalCard.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIgitalCard.Lib.Entities
{
    public class CardStatusRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CardId { get; set; } 
        public string OTP { get; set; } 
        public DateTime OTPExpiration { get; set; }
        public bool IsRequestCompleted { get; set; }

        public CardStatus CardStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? DateCompleted { get; set; }

    }
}
