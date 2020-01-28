using System.Collections.Generic;

namespace MaTReportingAPI.V1.Models
{
    public class ValidationResult
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool Valid { get; set; } = true;
    }
}
