using FluentValidation;
using MaTReportingAPI.V1.Boundary;
using System;
using System.Globalization;

namespace MaTReportingAPI.V1.Validators
{
    public class ListInteractionsAndChildInteractionsRequestValidator : AbstractValidator<ListInteractionsAndChildInteractionsRequest>
    {
        public ListInteractionsAndChildInteractionsRequestValidator()
        {
            RuleFor(x => x.FromDate).Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull()
                 .WithMessage("fromDate cannot be null")
                 .NotEmpty()
                 .WithMessage("fromDate cannot be empty")
                 .Must(IsValidDate)
                 .WithMessage("Please provide fromDate in valid format. Eg: 2019-04-01");

            RuleFor(x => x.ToDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("toDate cannot be null")
                .NotEmpty()
                .WithMessage("toDate cannot be empty")
                .Must(IsValidDate)
                .WithMessage("Please provide toDate in valid format. Eg: 2019-04-01");

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    if (IsValidDate(request.FromDate) && IsValidDate(request.ToDate))
                    {
                        if (!FromDateBeforeToDate(request))
                        {
                            context.AddFailure("fromDate cannot be after toDate");
                        }
                    }
                });
        }

        private bool IsValidDate(string providedDate)
        {
            return DateTime.TryParseExact(providedDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _);
        }

        private bool FromDateBeforeToDate(ListInteractionsAndChildInteractionsRequest arg)
        {
            DateTime fromDate = DateTime.ParseExact(arg.FromDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
            DateTime toDate = DateTime.ParseExact(arg.ToDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);

            return fromDate <= toDate ? true : false;
        }
    }
}
