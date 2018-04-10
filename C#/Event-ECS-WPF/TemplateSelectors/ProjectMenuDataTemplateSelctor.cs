using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.TemplateSelectors
{
    public class ProjectMenuDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Project project = item as Project;
            if (project != null)
            {
                Window window = Application.Current.MainWindow;

                if (project is LoveProject)
                {
                    return window.FindResource("loveProjectMenuTemplate") as DataTemplate;
                }
                return window.FindResource("projectMenuTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
