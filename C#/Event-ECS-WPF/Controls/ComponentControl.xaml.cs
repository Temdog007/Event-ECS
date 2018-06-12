using Event_ECS_Lib;
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
            if (ECS.Instance.UseWrapper(ReloadComponentFunc, out bool rval))
            {
                LogManager.Instance.Add(LogLevel.Medium, "Reloaded component {0} => {1}", Component.Name, rval);
                Component.Entity.Components.Remove(Component);
                Component.Entity.System.Deserialize();
            }
        }

        private bool ReloadComponentFunc(IECSWrapper ecs)
        {
            bool val;
            if (val = RemoveComponentFunc(ecs))
            {
                ecs.AddComponent(Component.Entity.System.Name, Component.Entity.ID, Component.Name);
            }
            else
            {
                LogManager.Instance.Add(LogLevel.High, "Failed to remove component {0} so it won't be reloaded", Component.Name);
            }
            return val;
        }

        private void RemoveComponent()
        {
            if (ECS.Instance.UseWrapper(RemoveComponentFunc, out bool rval))
            {
                LogManager.Instance.Add(LogLevel.Medium, "Removed component {0} => {1}", Component.Name, rval);
                Component.Entity.Components.Remove(Component);
                Component.Entity.System.Deserialize();
            }
        }

        private bool RemoveComponentFunc(IECSWrapper ecs)
        {
            return ecs.RemoveComponent(Component.Entity.System.Name, Component.Entity.ID, Component.ID);
        }

        private void SetComponentEnabled(bool enabled)
        {
            Component.IsEnabled = enabled;
        }
    }
}
