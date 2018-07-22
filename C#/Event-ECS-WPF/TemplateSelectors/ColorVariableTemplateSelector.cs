using Event_ECS_WPF.SystemObjects;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ColorVariableTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is KeyValuePair<string, IEntityVariable> pair)
            {
                if(pair.Value.Type == typeof(float))
                {
                    return Application.Current.FindResource("sliderEntityVariable") as DataTemplate;
                }
            }

            return null;
        }
    }
}
