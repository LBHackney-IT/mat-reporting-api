using System;
using System.Globalization;

namespace MaTReportingAPI.V1.Validators
{
    public static class Helpers
    {
        public static bool IsValidDate(string providedDate)
        {
            return DateTime.TryParseExact(providedDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _);
        }

        public static bool FromDateBeforeToDate(string fromDate, string toDate)
        {
            DateTime _fromDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);
            DateTime _toDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None);

            return _fromDate <= _toDate ? true : false;
        }
    }
}
