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
        private bool m_projectStarted = false;
        public ProjectMenuControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }
        public ICommand ProjectActionCommand => m_projectCommand ?? (m_projectCommand = new ActionCommand<object>(ProjectAction));

        public bool ProjectStarted
        {

            get => m_projectStarted;
            set
            {
                m_projectStarted = value;
                OnPropertyChanged("ProjectStarted");
                OnPropertyChanged("ProjectText");
            }
        }
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
                ProjectStarted = false;
            }
            else
            {
                ProjectStarted = Project.Start();
            }
        }
    }
}
