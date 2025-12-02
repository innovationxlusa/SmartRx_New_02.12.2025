using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PMSBackend.Application.CommonServices.Validation
{
    /// <summary>
    /// Validates string length only when the string is not null or empty
    /// </summary>
    public class OptionalStringLengthAttribute : ValidationAttribute
    {
        private readonly int _minimumLength;
        private readonly int _maximumLength;

        public OptionalStringLengthAttribute(int minimumLength, int maximumLength)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
        }

        public override bool IsValid(object? value)
        {
            // If value is null or empty, consider it valid (optional field)
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            var stringValue = value.ToString()!;
            
            // If string has content, validate length
            if (stringValue.Length < _minimumLength || stringValue.Length > _maximumLength)
            {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must be between {_minimumLength} and {_maximumLength} characters.";
        }
    }

    /// <summary>
    /// Validates regular expression only when the string is not null or empty
    /// </summary>
    public class OptionalRegularExpressionAttribute : ValidationAttribute
    {
        private readonly string _pattern;

        public OptionalRegularExpressionAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override bool IsValid(object? value)
        {
            // If value is null or empty, consider it valid (optional field)
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            var stringValue = value.ToString()!;
            
            // If string has content, validate pattern
            return Regex.IsMatch(stringValue, _pattern);
        }
    }

    /// <summary>
    /// Validates email format only when the string is not null or empty
    /// </summary>
    public class OptionalEmailAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // If value is null or empty, consider it valid (optional field)
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            var stringValue = value.ToString()!;
            
            // If string has content, validate email format
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(stringValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field is not a valid email address.";
        }
    }

    /// <summary>
    /// Validates range only when the value is not null
    /// </summary>
    public class OptionalRangeAttribute : ValidationAttribute
    {
        private readonly object _minimum;
        private readonly object _maximum;

        public OptionalRangeAttribute(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public OptionalRangeAttribute(double minimum, double maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public OptionalRangeAttribute(Type type, string minimum, string maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public override bool IsValid(object? value)
        {
            // If value is null, consider it valid (optional field)
            if (value == null)
            {
                return true;
            }

            // If value has content, validate range
            try
            {
                // Convert the value to a comparable type
                if (value is int intValue)
                {
                    var rangeAttribute = new RangeAttribute((int)_minimum, (int)_maximum);
                    return rangeAttribute.IsValid(value);
                }
                else if (value is decimal decimalValue)
                {
                    var rangeAttribute = new RangeAttribute(Convert.ToDouble(_minimum), Convert.ToDouble(_maximum));
                    return rangeAttribute.IsValid(value);
                }
                else if (value is double doubleValue)
                {
                    var rangeAttribute = new RangeAttribute(Convert.ToDouble(_minimum), Convert.ToDouble(_maximum));
                    return rangeAttribute.IsValid(value);
                }
                else
                {
                    // For other numeric types, try to convert to double
                    var rangeAttribute = new RangeAttribute(Convert.ToDouble(_minimum), Convert.ToDouble(_maximum));
                    return rangeAttribute.IsValid(value);
                }
            }
            catch
            {
                // If conversion fails, consider it invalid
                return false;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field must be between {_minimum} and {_maximum}.";
        }
    }
}
