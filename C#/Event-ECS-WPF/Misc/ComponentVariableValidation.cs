using Event_ECS_WPF.SystemObjects;
using System.Globalization;
using System.Windows.Controls;

namespace Event_ECS_WPF.Misc
{
    public class ComponentVariableValidation : ValidationRule
    {
        public IComponentVariable Variable { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Variable.Type != typeof(string))
            {
                if (Variable.Type == typeof(bool))
                {
                    if (!bool.TryParse(value.ToString(), out bool result))
                    {
                        return new ValidationResult(false, "Can't convert to bool");
                    }
                }
                else if (Variable.Type == typeof(float))
                {
                    if(!float.TryParse(value.ToString(), out float result))
                    {
                        return new ValidationResult(false, "Can't convert to float");
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
