using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
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

        private ICommand m_setComponentEnabledCommand;

        private ICommand m_removeComponentCommand;

        public ComponentControl()
        {
            InitializeComponent();
        }

        public Component Component
        {
            get { return (Component)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }

        public ICommand SetComponentEnabledCommand => m_setComponentEnabledCommand ?? (m_setComponentEnabledCommand = new ActionCommand<bool>(SetComponentEnabled));
        
        private void SetComponentEnabled(bool enabled)
        {
            Component.IsEnabled = enabled;
        }

        public ICommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand(RemoveComponent));

        private void RemoveComponent()
        {
            if(ECS.Instance.UseWrapper(RemoveComponentFunc, out bool rval))
            {
                LogManager.Instance.Add(LogLevel.Medium, "Removed component {0} => {1}", Component.Name, rval);
                Component.Entity.Components.Remove(Component);
                Component.Entity.System.Deserialize();
            }
        }

        private bool RemoveComponentFunc(ECSWrapper ecs)
        {
            return ecs.RemoveComponent(Component.Entity.System.Name, Component.Entity.ID, Component.ID);
        }
    }
}
