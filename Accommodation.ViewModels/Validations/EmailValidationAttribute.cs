using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Octupus.ViewModels.Validations
{
    [ExcludeFromCodeCoverage]
    public class EmailValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string pattern = @"^([\w\.\-]+)@([\w\-]+\.)+(?:[A-Z]{2,3}|sex|xxx|aero|asia|cat|coop|edu|gov|int|jobs|mil|mobi|museum|music|post|tel|travel|info)$";
            bool result = Regex.IsMatch(value.ToString(), pattern, RegexOptions.IgnoreCase);

            return result;
        }
    }
}
