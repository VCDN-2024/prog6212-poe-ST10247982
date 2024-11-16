using FluentValidation;
using POE_p2_s4.Models;

namespace POE_p2_s4.Validation
{
    public class CourseValidation: AbstractValidator<Course>
    {
        public CourseValidation()
        {
           
            RuleFor(course => course.Name).Length(3, 50).NotNull().WithMessage("Please enter a {PropertyName} name with 3-50 character");
            RuleFor(course => course.Department).Length(3, 50).NotNull().WithMessage("Please enter a {PropertyName} name with {Minlength} - {MaxLength} character");
            RuleFor(course => course.UserId).Length(255).NotNull().WithMessage("No {PropertyName} associated with this course. ");
        }
    }
}
