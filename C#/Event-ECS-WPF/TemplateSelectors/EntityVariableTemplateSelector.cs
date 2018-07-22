using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.SystemObjects;
using Event_ECS_WPF.SystemObjects.EntityAttributes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class EntityVariableTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            string resourceName = "defaultEntityVariable";
            if (item is IEntityVariable variable)
            {
                if (variable.Name.Contains("color", StringComparison.OrdinalIgnoreCase))
                {
                    resourceName = "colorEntityVariable";
                }
                else
                {
                    var type = variable.Type;
                    if (type == typeof(bool))
                    {
                        resourceName = "boolEntityVariable";
                    }
                    else if(type == typeof(LuaTable))
                    {
                        resourceName = "tableEntityVariable";
                    }
                }
            }
        
            return Application.Current.FindResource(resourceName) as DataTemplate;
        }
    }
}
