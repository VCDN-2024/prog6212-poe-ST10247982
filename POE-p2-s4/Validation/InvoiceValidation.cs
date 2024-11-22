using FluentValidation;
using POE_p2_s4.Models;

namespace POE_p2_s4.Validation
{
    public class InvoiceValidation:AbstractValidator<Invoice>
    {
        public InvoiceValidation() 
        {

            RuleFor(invoice => invoice.Id).Length(33, 50).NotNull().WithMessage("Please enter a {PropertyName}  with {Minlength} - {MaxLength} character");
            RuleFor(invoice => invoice.Title).Length(4, 30).NotNull().WithMessage("Please enter a {PropertyName} name with {Minlength} - {MaxLength} character");
            RuleFor(invoice => invoice.UserId).Length(33, 50).NotNull().WithMessage("No {PropertyName} associated with this course. ");
            RuleFor(invoice => invoice.IsPaymentMade).NotNull().WithMessage("No {PropertyName} specification made! ");
            RuleFor(invoice => invoice.Claims).NotEmpty().WithMessage("Not a single claim in the list of {PropertyName}!");
            RuleFor(invoice => invoice.CreatedDate).NotNull().WithMessage("There is no valid {Propertyname} for this invoice!");
        }
    }
}
