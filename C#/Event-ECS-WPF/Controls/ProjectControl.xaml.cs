using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public ICommand ButtonClickCommand => m_buttonClickCommand ?? (m_buttonClickCommand = new ActionCommand<string>(Button_Click));
        private ActionCommand<string> m_buttonClickCommand;

        private void Button_Click(string propertyName)
        {
            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                switch(dialog.ShowDialog())
                {
                    case System.Windows.Forms.DialogResult.OK:
                        if(!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            Project.SetProperty(propertyName, dialog.SelectedPath);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
