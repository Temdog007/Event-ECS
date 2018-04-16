using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ComponentVariableTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ComponentVariable variable = item as ComponentVariable;
            if(variable != null)
            {
                Window window = Application.Current.MainWindow;

                if (variable.Type.IsAssignableFrom(typeof(bool)))
                {
                    return window.FindResource("boolTemp") as DataTemplate;
                }
                else if(variable.Name == "id")
                {
                    return window.FindResource("readOnlyTemp") as DataTemplate;
                }
                return window.FindResource("valueTemp") as DataTemplate;
            }
            return null;
        }
    }
}
