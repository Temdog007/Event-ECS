using Event_ECS_WPF.SystemObjects;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ComponentTemplateSelector : DataTemplateSelector
    {
        private static readonly string[] colors = { "r", "g", "b", "a" };

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IComponentVariable variable)
            {
                var app = Application.Current;

                if(variable.Component.Name == "ColorComponent" && colors.Any(c => c == variable.Name) && variable.Type == typeof(float))
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
