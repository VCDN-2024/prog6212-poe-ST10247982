using FluentValidation;
using POE_p2_s4.Models;
using POE_p2_s4.ViewModels;
using System.Security.Claims;
using Claim = POE_p2_s4.Models.Claim;

namespace POE_p2_s4.Validation
{
    public class ClaimValidation:AbstractValidator<Claim>
    {
        private readonly decimal _hourlyRate;
        const int maxFileSize = 5 * 1024 * 1024;

        private readonly decimal _foodExpenseMultiplier = 0.4m;  
 
        
        public ClaimValidation(decimal HourlYRate)
        {
            _hourlyRate = HourlYRate;
            RuleFor(claim => claim.ClaimType).Length(3, 50).NotNull().WithMessage("Please enter a {PropertyName} name with {Minlength} - {MaxLength} character");
            RuleFor(claim => claim.Description).Length(3, 50).NotNull().WithMessage("Please enter a {PropertyName} name with {Minlength} - {MaxLength} character");
            RuleFor(claim => claim.ClaimType).Length(3, 50).NotNull().WithMessage("Please enter a {PropertyName} name with {Minlength} - {MaxLength} character");
            RuleFor(claim => claim.ClaimDate).NotNull().WithMessage("Please enter a valid date");
            RuleFor(claim => claim.HoursWorked).GreaterThan(0).LessThan(40).NotNull().WithMessage("Please enter a valid work hour amount ");
            RuleFor(claim => claim.KilometersTravelled).GreaterThan(0).WithMessage("Please enter a distance greater than 0");
            RuleFor(claim => claim.LeaveDays).GreaterThan(0).WithMessage("Please enter a value greater then 0 for days of leave");
           
            RuleFor(claim => claim)
                .Must(ValidateDocumentBinary)
                .WithMessage(claim => $"The document is bigger than {maxFileSize}!").When(claim => claim.DocumentBinary.Length > 0);

            RuleFor(claim => claim)
               .Must(ValidateClaimAmount)
               .WithMessage(claim => $"The {claim.ClaimType} expense exceeds the allowable limit based on hours worked and hourly rate.");


        }
        private bool ValidateDocumentBinary(Claim claim)
        {
            if (claim.DocumentBinary == null)
            {
                return false;
            }
            return claim.DocumentBinary.Length < maxFileSize;   // will return true if less then max else false
        }
        private bool ValidateClaimAmount(Claim claim)
        {
            if (claim.HoursWorked<=40 && claim.HoursWorked>0)
            {
                
                return false;
            }

          
            decimal maxAllowableExpense = (decimal)claim.HoursWorked * _hourlyRate;

            switch (claim.ClaimType)
            {
                case "Food":
                    return claim.ClaimExpenses <=(double) (maxAllowableExpense * _foodExpenseMultiplier + maxAllowableExpense);

                case "Travel":
                    return claim.ClaimExpenses <=(double) (maxAllowableExpense *  claim.KilometersTravelled +maxAllowableExpense);
                case "Leave":
                    return claim.ClaimExpenses <= (double)(maxAllowableExpense *claim.LeaveDays+ maxAllowableExpense);
                
                default:
                    
                    return false;
            }
        }
}
      
}
