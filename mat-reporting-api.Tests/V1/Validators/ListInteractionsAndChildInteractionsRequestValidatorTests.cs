using FluentValidation;
using FluentValidation.TestHelper;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Validators;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Validators
{
    public class ListInteractionsAndChildInteractionsRequestValidatorTests : AbstractValidator<ListInteractionsAndChildInteractionsRequest>
    {
        private readonly ListInteractionsAndChildInteractionsRequestValidator _listInteractionsAndChildInteractionsRequestValidator;

        private ListInteractionsAndChildInteractionsRequest GetValidListInterActionsAndChildInteractionsRequest()
        {
            return new ListInteractionsAndChildInteractionsRequest()
            {
                FromDate = "2019-04-01",
                ToDate = "2019-04-30"
            };
        }

        public ListInteractionsAndChildInteractionsRequestValidatorTests()
        {
            _listInteractionsAndChildInteractionsRequestValidator = new ListInteractionsAndChildInteractionsRequestValidator();
        }

        #region FromDate
        [Fact]
        public void ValidatorReturnsTrueForValidFromDateFormat()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            _listInteractionsAndChildInteractionsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidFromDateFormat()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.FromDate = "01-01-2020";
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForMissingFromDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.FromDate = null;
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyFromDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.FromDate = "";
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }
        #endregion

        #region Todate
        [Fact]
        public void ValidatorReturnsTrueForValidToDateFormat()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            _listInteractionsAndChildInteractionsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidToDateFormat()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.ToDate = "01-01-2020";
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        [Fact]
        public void ValidatorReturnsFalseForMissingToDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.ToDate = null;
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyToDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.ToDate = "";
            _listInteractionsAndChildInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        #endregion

        [Fact]
        public void ValidatorReturnsTrueForFromDateBeingBeforeToDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            var validationResult = _listInteractionsAndChildInteractionsRequestValidator.Validate(request);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ValidatorReturnsFalseForFromDateBeingAfterToDate()
        {
            ListInteractionsAndChildInteractionsRequest request = GetValidListInterActionsAndChildInteractionsRequest();
            request.FromDate = "2019-04-30";
            request.ToDate = "2019-04-01";
            var validationResult = _listInteractionsAndChildInteractionsRequestValidator.Validate(request);
            Assert.False(validationResult.IsValid);
            Assert.Equal("fromDate cannot be after toDate", validationResult.Errors[0].ErrorMessage);
        }
    }
}
