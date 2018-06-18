using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for LoveLoveProjectMenuControl.xaml
    /// </summary>
    public partial class LoveProjectMenuControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LoveProjectProperty =
            DependencyProperty.Register("LoveProject", typeof(LoveProject), typeof(LoveProjectMenuControl));

        private ICommand m_updateCommand;
        
        public LoveProjectMenuControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoveProject LoveProject
        {
            get { return (LoveProject)GetValue(LoveProjectProperty); }
            set { SetValue(LoveProjectProperty, value); }
        }

        public ICommand UpdateCommand => m_updateCommand ?? (m_updateCommand = new ActionCommand(Update));

        public void Update()
        {
            if (ECS.Instance.IsApplicationRunning && ECS.Instance.IsUpdatingAutomaticallyProperty)
            {
                ECS.Instance.LoveUpdate();
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
