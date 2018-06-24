using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ComponentControl.xaml
    /// </summary>
    public partial class ComponentControl : UserControl
    {
        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("Component", typeof(Component), typeof(ComponentControl));

        private ICommand m_reloadComponentCommand;

        private ICommand m_removeComponentCommand;

        private ICommand m_setComponentEnabledCommand;

        public ComponentControl()
        {
            InitializeComponent();
        }

        public Component Component
        {
            get { return (Component)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }

        public ICommand ReloadComponentCommand => m_reloadComponentCommand ?? (m_reloadComponentCommand = new ActionCommand(ReloadComponent));

        public ICommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand(RemoveComponent));

        public ICommand SetComponentEnabledCommand => m_setComponentEnabledCommand ?? (m_setComponentEnabledCommand = new ActionCommand<bool>(SetComponentEnabled));
        
        private void ReloadComponent()
        {
            RemoveComponent();
            ECS.Instance.AddComponent(Component.Entity.System.Name, Component.Entity.ID, Component.Name);
            LogManager.Instance.Add(LogLevel.Medium, "Added component {0}", Component.Name);
        }

        private void RemoveComponent()
        {
            ECS.Instance.RemoveComponent(Component.Entity.System.Name, Component.Entity.ID, Component.ID);
            LogManager.Instance.Add(LogLevel.Medium, "Removed component {0}", Component.Name);
        }

        private void SetComponentEnabled(bool enabled)
        {
            ECS.Instance.SetComponentEnabled(Component.Entity.System.Name, Component.Entity.ID, Component.ID, enabled);
        }
    }
}