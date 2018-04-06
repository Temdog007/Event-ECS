using System;
using System.Windows.Markup;

namespace Event_ECS_Client_WPF
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type type)
        {
            m_enumType = type;
        }

        public Type EnumType
        {
            get => m_enumType;
            set
            {
                if(m_enumType == value)
                {
                    return;
                }

                if(value != null)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if(!EnumType.IsEnum)
                    {
                        throw new ArgumentException("Type must be for an Enum.");
                    }
                }

                m_enumType = value;
            }
        }
        private Type m_enumType;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if(m_enumType == null)
            {
                throw new InvalidOperationException("The EnumType must be specified");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(m_enumType) ?? m_enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if(actualEnumType == m_enumType)
            {
                return enumValues;
            }

            Array temp = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(temp, 1);
            return temp;
        }
    }
}
