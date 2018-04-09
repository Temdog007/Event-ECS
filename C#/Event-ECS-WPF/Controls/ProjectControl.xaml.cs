using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public ProjectControl()
        {
            InitializeComponent();
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectControl));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                switch(dialog.ShowDialog())
                {
                    case System.Windows.Forms.DialogResult.OK:
                        if(!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            Project.ComponentPath = dialog.SelectedPath;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
