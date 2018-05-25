using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System.Collections.Generic;
using System.ComponentModel;
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

        public IActionCommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<string>(AddComponent));

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

        public bool ShowEvents
        {
            get => m_showEvents;
            set
            {
                m_showEvents = value;
                OnPropertyChanged("ShowEvents");
            }
        }

        private bool RemoveFunc(ECSWrapper ecs)
        {
            return ecs.RemoveEntity(Entity.System.Name, Entity.ID);
        }

        private void Remove()
        {
            if(ECS.Instance.UseWrapper(RemoveFunc, out bool rval))
            {
                LogManager.Instance.Add("Removed entity: {0}", rval);
                Entity.System.Deserialize();
            }
        }

        public static IEnumerable<string> Alphabet
        {
            get
            {
                for(char c = 'A'; c <= 'Z'; ++c)
                {
                    yield return c.ToString();
                }
            }
        }

        public ICommand RemoveCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand(Remove));

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddComponent(string param)
        {
            ECS.Instance.UseWrapper(ecs => ecs.AddComponent(Entity.System.Name, Entity.ID, param));
            Entity.System.Deserialize();
        }

        private void Entity_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddComponentCommand.UpdateCanExecute(this, e);
        }
    }
}
