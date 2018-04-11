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

        private UpdateType m_type = UpdateType.Manual;

        private ICommand m_updateCommand;

        public LoveProjectMenuControl()
        {
            InitializeComponent();

            LoveProject.ProjectStateChange += Value_ProjectStateChange;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanUpdate => UpdateType == UpdateType.Manual;

        public LoveProject LoveProject
        {
            get { return (LoveProject)GetValue(LoveProjectProperty); }
            set { SetValue(LoveProjectProperty, value); }
        }

        private void Value_ProjectStateChange(object sender, ProjectStateChangeArgs args)
        {
            if(args.IsStarted && UpdateType == UpdateType.Automatic)
            {
                ECS.Instance.SetAutoUpdate(true);
            }
        }

        public ICommand UpdateCommand => m_updateCommand ?? (m_updateCommand = new ActionCommand<object>(Update));

        public UpdateType UpdateType
        {
            get => m_type;
            set
            {
                if(m_type == value)
                {
                    return;
                }

                m_type = value;
                if (ECS.Instance != null)
                {
                    switch (value)
                    {
                        case UpdateType.Manual:
                            ECS.Instance.SetAutoUpdate(false);
                            break;
                        case UpdateType.Automatic:
                            ECS.Instance.SetAutoUpdate(true);
                            break;
                    }
                }
                OnPropertyChanged("UpdateType");
                OnPropertyChanged("CanUpdate");
            }
        }
        public void Update()
        {
            switch (UpdateType)
            {
                case UpdateType.Manual:
                    ECS.Instance?.Update();
                    break;
                default:
                    break;
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
