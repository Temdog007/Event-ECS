using Event_ECS_WPF.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Misc
{
    public class ProjectDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Project project = item as Project;
            if(project != null)
            {
                Window window = Application.Current.MainWindow;

                if(project is LoveProject)
                {
                    return window.FindResource("loveProjectTemplate") as DataTemplate;
                }
                return window.FindResource("projectTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
