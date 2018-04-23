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

            Project.ProjectStateChange += Value_ProjectStateChange;
            ECS.OnAutoUpdateChanged += OnAutoUpdateChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanUpdate => UpdateType == UpdateType.Automatic;

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

        private void OnAutoUpdateChanged(object sender, AutoUpdateChangedArgs e)
        {
            OnPropertyChanged("UpdateType");
            OnPropertyChanged("CanUpdate");
        }

        public ICommand UpdateCommand => m_updateCommand ?? (m_updateCommand = new ActionCommand(Update));

        public UpdateType UpdateType
        {
            get => ECS.Instance.GetAutoUpdate() ? UpdateType.Automatic : UpdateType.Manual;
            set
            {
                if (ECS.Instance == null)
                {
                    return;
                }

                if ((value == UpdateType.Automatic && ECS.Instance.GetAutoUpdate()) || (value == UpdateType.Manual && !ECS.Instance.GetAutoUpdate()))
                {
                    return;
                }
                
                switch (value)
                {
                    case UpdateType.Manual:
                        ECS.Instance.SetAutoUpdate(false);
                        break;
                    case UpdateType.Automatic:
                        ECS.Instance.SetAutoUpdate(true);
                        break;
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
                    ECS.Instance.Update();
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
