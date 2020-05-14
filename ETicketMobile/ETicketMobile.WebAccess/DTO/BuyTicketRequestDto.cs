﻿namespace ETicketMobile.WebAccess.DTO
{
    public class BuyTicketRequestDto
    {
        public int TicketTypeId { get; set; }

        public string Email { get; set; }

        public decimal Coefficient { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }

        public string ExpirationMonth { get; set; }

        public string ExpirationYear { get; set; }

        public string CVV2 { get; set; }
    }
}