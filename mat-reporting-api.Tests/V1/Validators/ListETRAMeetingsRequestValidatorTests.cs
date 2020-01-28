using FluentValidation.TestHelper;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Validators
{
    public class ListETRAMeetingsRequestValidatorTests
    {
        private readonly ListETRAMeetingsRequestValidator _listETRAMeetingsRequestValidator;

        private ListETRAMeetingsRequest GetValidListETRAMeetingsRequest()
        {
            return new ListETRAMeetingsRequest()
            {
                FromDate = "2019-04-01",
                ToDate = "2019-04-30"
            };
        }

        public ListETRAMeetingsRequestValidatorTests()
        {
            _listETRAMeetingsRequestValidator = new ListETRAMeetingsRequestValidator();
        }

        #region FromDate
        [Fact]
        public void ValidatorReturnsTrueForValidFromDateFormat()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            _listETRAMeetingsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidFromDateFormat()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.FromDate = "01-01-2020";
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForMissingFromDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.FromDate = null;
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyFromDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.FromDate = "";
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.FromDate, request.FromDate);
        }
        #endregion

        #region Todate
        [Fact]
        public void ValidatorReturnsTrueForValidToDateFormat()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            _listETRAMeetingsRequestValidator.ShouldNotHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForInvalidToDateFormat()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.ToDate = "01-01-2020";
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        [Fact]
        public void ValidatorReturnsFalseForMissingToDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.ToDate = null;
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }

        [Fact]
        public void ValidatorReturnsFalseForEmptyToDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.ToDate = "";
            _listETRAMeetingsRequestValidator.ShouldHaveValidationErrorFor(r => r.ToDate, request.ToDate);
        }
        #endregion

        [Fact]
        public void ValidatorReturnsTrueForFromDateBeingBeforeToDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            var validationResult = _listETRAMeetingsRequestValidator.Validate(request);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ValidatorReturnsFalseForFromDateBeingAfterToDate()
        {
            ListETRAMeetingsRequest request = GetValidListETRAMeetingsRequest();
            request.FromDate = "2019-04-30";
            request.ToDate = "2019-04-01";
            var validationResult = _listETRAMeetingsRequestValidator.Validate(request);
            Assert.False(validationResult.IsValid);
            Assert.Equal("fromDate cannot be after toDate", validationResult.Errors[0].ErrorMessage);
        }
    }
}
