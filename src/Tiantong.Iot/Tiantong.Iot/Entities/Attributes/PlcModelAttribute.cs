using System.ComponentModel.DataAnnotations;

namespace Tiantong.Iot.Entities
{
  public class PlcModelAttribute: ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
      if (PlcModel.IsValid(value.ToString())) {
        return new ValidationResult("PLC 型号错误");
      } else {
        return ValidationResult.Success;
      }
    }
  } 
}