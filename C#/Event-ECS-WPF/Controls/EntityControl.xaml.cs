using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
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
        public EntityControl()
        {
            InitializeComponent();
        }

        public Entity Entity
        {
            get { return (Entity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public SystemObjects.Component SelectedComponent
        {
            get => m_component;
            set
            {
                m_component = value;
                OnPropertyChanged("SelectedComponent");
            }
        } private SystemObjects.Component m_component;

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(EntityControl));

        public IActionCommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<string>(AddComponent));
        private IActionCommand m_addComponentCommand;

        public IActionCommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand(RemoveComponent, CanRemoveComponent));
        private IActionCommand m_removeComponentCommand;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool RemoveSelectedComponent(ECSWrapper ecs)
        {
            try
            {
                return ecs.RemoveComponent(Entity.ID, SelectedComponent.ID);
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, "Cannot remove component: {0}", e.Message);
                return false;
            }
        }

        private void AddComponent(string param)
        {
            ECS.Instance.UseWrapper(ecs => ecs.AddComponent(Entity.ID, param));
            Entity.System.Deserialize();
        }

        private bool CanRemoveComponent()
        {
            return SelectedComponent != null;
        }

        private void RemoveComponent()
        {
            ECS.Instance.UseWrapper(RemoveSelectedComponent, out bool rval);
            Entity.System.Deserialize();
        }

        private void Entity_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddComponentCommand.UpdateCanExecute(this, e);
            RemoveComponentCommand.UpdateCanExecute(this, e);
        }
    }
}
