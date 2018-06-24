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

        private ICommand m_startCommand;

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

        public ICommand StartCommand => m_startCommand ?? (m_startCommand = new ActionCommand(ProjectStart));

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ProjectStart()
        {
            Project.Start();
        }
    }
}
