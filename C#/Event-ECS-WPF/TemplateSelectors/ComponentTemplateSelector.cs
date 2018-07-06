using Event_ECS_WPF.SystemObjects;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ComponentTemplateSelector : DataTemplateSelector
    {
        private static readonly string[] colors = { "1", "2", "3", "4" };

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IComponentVariable variable)
            {
                var app = Application.Current;
                
                if(variable.Name.Equals("ID", StringComparison.OrdinalIgnoreCase))
                {
                    return app.FindResource("idComponentVariable") as DataTemplate;
                }
                if (variable.Component.Name.Equals("ColorComponent", StringComparison.OrdinalIgnoreCase) && variable.Type == typeof(float))
                {
                    return app.FindResource("colorComponentVariable") as DataTemplate;
                }
                else if(variable.Type == typeof(bool))
                {
                    return app.FindResource("boolComponentVariable") as DataTemplate;
                }
                return app.FindResource("defaultComponentVariable") as DataTemplate;
            }
            return null;
        }
    }
}
