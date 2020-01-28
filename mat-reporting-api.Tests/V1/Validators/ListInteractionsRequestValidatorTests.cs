using FluentValidation.TestHelper;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Validators;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Validators
{
    public class ListInteractionsRequestValidatorTests
    {
        private readonly ListInteractionsRequestValidator _listInteractionsRequestValidator;

        private ListInteractionsRequest GetValidListInterActionsRequest()
        {
            return new ListInteractionsRequest()
            {
                FromDate = "2019-04-01",
                ToDate = "2019-04-30"
            };
        }

        public ListInteractionsRequestValidatorTests()
        {
            _listInteractionsRequestValidator = new ListInteractionsRequestValidator();
        }

        #region FromDate
        [Fact]
        public void ValidatorReturnsTrueForValidFromDateFormat()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            _listInteractionsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidFromDateFormat()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.FromDate = "01-01-2020";
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForMissingFromDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.FromDate = null;
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyFromDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.FromDate = "";
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }
        #endregion

        #region Todate
        [Fact]
        public void ValidatorReturnsTrueForValidToDateFormat()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            _listInteractionsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidToDateFormat()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.ToDate = "01-01-2020";
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        [Fact]
        public void ValidatorReturnsFalseForMissingToDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.ToDate = null;
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyToDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.ToDate = "";
            _listInteractionsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        #endregion

        [Fact]
        public void ValidatorReturnsTrueForFromDateBeingBeforeToDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            var validationResult = _listInteractionsRequestValidator.Validate(request);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ValidatorReturnsFalseForFromDateBeingAfterToDate()
        {
            ListInteractionsRequest request = GetValidListInterActionsRequest();
            request.FromDate = "2019-04-30";
            request.ToDate = "2019-04-01";
            var validationResult = _listInteractionsRequestValidator.Validate(request);
            Assert.False(validationResult.IsValid);
            Assert.Equal("fromDate cannot be after toDate", validationResult.Errors[0].ErrorMessage);
        }
    }
}
