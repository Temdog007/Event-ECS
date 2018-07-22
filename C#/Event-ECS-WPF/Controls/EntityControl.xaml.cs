using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using Event_ECS_WPF.SystemObjects.Communication;
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
        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(EntityControl));

        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(EntityControl));

        private IActionCommand m_addComponentCommand;

        private IActionCommand m_removeEntityCommand;

        private string m_selectedEvent = string.Empty;

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

        public string SelectedEvent
        {
            get => m_selectedEvent;
            set
            {
                m_selectedEvent = value;
                OnPropertyChanged("SelectedEvent");
            }
        } 
        
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddComponent(string compName)
        {
            ECS.Instance.AddComponent(Entity.System.ID, Entity.ID, compName);
        }

        private void DispatchButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(SelectedEvent))
            {
                return;
            }
            string ev = SelectedEvent;
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.DispatchEvent(Entity.System.ID, Entity.ID, ev);
        }

        private void Expander_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Expander expander)
            {
                expander.IsExpanded = true;
            }
        }

        private void Remove()
        {
           ECS.Instance.RemoveEntity(Entity.System.ID, Entity.ID);
        }

        private void RemoveComponent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is SystemObjects.Component component)
            {
                ECS.Instance.RemoveComponent(Entity.System.ID, Entity.ID, component.ID);
            }
        }
    }
}
