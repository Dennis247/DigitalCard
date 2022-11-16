using DIgitalCard.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIgitalCard.Lib.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PAN { get; set; }
        public string ExpiryDate { get; set; }
        public string CCV { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public CardStatus CardStatus { get; set; }
        public string CardName { get; set; }

    }
}
