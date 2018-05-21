using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ProjectDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Project project)
            {
                var app = Application.Current;

                if (project is LoveProject)
                {
                    return app.FindResource("loveProjectTemplate") as DataTemplate;
                }
                return app.FindResource("projectTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
