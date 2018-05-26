using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for Entity.xaml
    /// </summary>
    public partial class EntityControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(EntityControl));

        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(EntityControl));

        private IActionCommand m_addComponentCommand;

        private IActionCommand m_removeEntityCommand;

        private bool m_showEvents = false;
        public EntityControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<string>(AddComponent));

        public Entity Entity
        {
            get { return (Entity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }
        public ICommand RemoveCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand(Remove));

        public bool ShowEvents
        {
            get => m_showEvents;
            set
            {
                m_showEvents = value;
                OnPropertyChanged("ShowEvents");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddComponent(string compName)
        {
            ECS.Instance.UseWrapper(ecs => ecs.AddComponent(Entity.System.Name, Entity.ID, compName));
            Entity.System.Deserialize();
        }
        
        private void Remove()
        {
            if (ECS.Instance.UseWrapper(RemoveFunc, out bool rval))
            {
                LogManager.Instance.Add("Removed entity: {0}", rval);
                Entity.System.Deserialize();
            }
        }

        private bool RemoveFunc(ECSWrapper ecs)
        {
            return ecs.RemoveEntity(Entity.System.Name, Entity.ID);
        }
    }
}
