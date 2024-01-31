using System.ComponentModel.DataAnnotations;
using Track3_Api_Interfaces_Core.Consts;

namespace Track3_Api_Interfaces_Core.Validators
{
    public class MethodAttributeValidator: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string _value = value as string;
            if(HTTPMETHODS.METHODS.Contains(_value))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"El campo {validationContext.DisplayName} debe ser un metodo HTTP valido.");
        }
    }
}
