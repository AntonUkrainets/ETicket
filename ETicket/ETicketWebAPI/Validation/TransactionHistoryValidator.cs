using System.Linq;
using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.WebAPI.Validation
{
    public class TransactionHistoryValidator : AbstractValidator<TransactionHistoryDto>
    {
        public TransactionHistoryValidator()
        {
            RuleFor(t => t.Count)
                .GreaterThan(0);

            RuleFor(t => t.TicketTypeId)
                .NotEmpty();

            RuleFor(t => t.TotalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThan(decimal.Zero);

            RuleFor(t => t.ReferenceNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(13)
                .Must(t=>t.All(char.IsNumber));
        }
    }
}