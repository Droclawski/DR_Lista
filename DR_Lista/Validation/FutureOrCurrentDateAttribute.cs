using System;
using System.ComponentModel.DataAnnotations;

namespace DR_Lista.Validation
{
    public class FutureOrCurrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date.Date < DateTime.Today)
                {
                    return new ValidationResult("The date cannot be in the past.");
                }
            }
            return ValidationResult.Success;
        }
    }
}