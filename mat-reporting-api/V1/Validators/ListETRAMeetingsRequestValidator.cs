using FluentValidation;
using MaTReportingAPI.V1.Boundary;

namespace MaTReportingAPI.V1.Validators
{
    public class ListETRAMeetingsRequestValidator : AbstractValidator<ListETRAMeetingsRequest>
    {
        public ListETRAMeetingsRequestValidator()
        {
            RuleFor(x => x.FromDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("fromDate cannot be null")
                .NotEmpty()
                .WithMessage("fromDate cannot be empty")
                .Must(Helpers.IsValidDate)
                .WithMessage("Please provide fromDate in valid format. Eg: 2019-04-01");

        RuleFor(x => x.ToDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("toDate cannot be null")
                .NotEmpty()
                .WithMessage("toDate cannot be empty")
                .Must(Helpers.IsValidDate)
                .WithMessage("Please provide toDate in valid format. Eg: 2019-04-01");

        RuleFor(x => x)
                .Custom((request, context) =>
                 {
            if (Helpers.IsValidDate(request.FromDate) && Helpers.IsValidDate(request.ToDate))
            {
                if (!Helpers.FromDateBeforeToDate(request.FromDate, request.ToDate))
                {
                    context.AddFailure("fromDate cannot be after toDate");
                }
            }
        });
        }
    }
}
