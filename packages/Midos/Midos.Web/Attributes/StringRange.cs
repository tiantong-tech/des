using System.ComponentModel.DataAnnotations;

namespace Midos.Web
{
  public class StringRangeAttribute: ValidationAttribute
  {
    private int _min;

    private int _max;

    public StringRangeAttribute(int min, int max)
    {
      _min = min;
      _max = max;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is null) {
        return new ValidationResult(ErrorMessage);
      }

      var length = value.ToString().Length;

      if (length >= _min && length <= _max) {
        return ValidationResult.Success;
      } else {
        return new ValidationResult(ErrorMessage);
      }
    }
  }
}
