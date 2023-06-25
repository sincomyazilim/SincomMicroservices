using FluentValidation;
using FreeCourse.Web.Models.Discount;//161

namespace FreeCourse.Web.Validator
{
    public class DiscountApplyInputValidator:AbstractValidator<DiscountApplyInputCode>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("indirim kupon alanı boş olamaz");
        }
    }
}
