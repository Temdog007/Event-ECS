using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Projects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectMenuControl.xaml
    /// </summary>
    public partial class ProjectMenuControl : UserControl, INotifyPropertyChanged
    {

        public static readonly DependencyProperty ProjectProperty =
                            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectMenuControl));
        private ICommand m_projectCommand;
        public ProjectMenuControl()
        {
            InitializeComponent();
            Project.ProjectStateChange += Project_ProjectStateChange;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private void Project_ProjectStateChange(object sender, ProjectStateChangeArgs args)
        {
            OnPropertyChanged("ProjectStarted");
            OnPropertyChanged("ProjectText");
        }

        public ICommand ProjectActionCommand => m_projectCommand ?? (m_projectCommand = new ActionCommand(ProjectAction));

        public bool ProjectStarted => Project?.IsStarted ?? false;

        public string ProjectText => ProjectStarted ? " Stop" : "Start";
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void ProjectAction()
        {
            if (ProjectStarted)
            {
                Project.Stop();
            }
            else
            {
                Project.Start();
            }
        }
    }
}
