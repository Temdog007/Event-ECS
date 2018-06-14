using Event_ECS_Lib;
using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using System.Threading;
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

        private ManualResetEvent removedComponentEvent = new ManualResetEvent(false);

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
            ECS.Instance.UseWrapper(ReloadComponentFunc, ReloadComponentResponse);
        }

        private void ReloadComponentFunc(IECSWrapper ecs)
        {
            RemoveComponentFunc(ecs);
            if (removedComponentEvent.WaitOne(100))
            {
                ecs.AddComponent(Component.Entity.System.Name, Component.Entity.ID, Component.Name);
            }
        }

        private bool ReloadComponentResponse(string funcName, object rval)
        {
            if (funcName == "ReloadComponent")
            {
                LogManager.Instance.Add(LogLevel.Medium, "Reloaded component {0} => {1}", Component.Name, rval);
                Component.Entity.Components.Remove(Component);
                Component.Entity.System.Deserialize();
                return true;
            }
            return false;
        }

        private void RemoveComponent()
        {
            ECS.Instance.UseWrapper(RemoveComponentFunc, RemoveComponentResponse);
        }

        private void RemoveComponentFunc(IECSWrapper ecs)
        {
            removedComponentEvent.Reset();
            ecs.RemoveComponent(Component.Entity.System.Name, Component.Entity.ID, Component.ID);
        }

        private bool RemoveComponentResponse(string funcName, object rval)
        {
            if (funcName == "RemoveComponent")
            {
                if (rval is bool removed && removed)
                {
                    removedComponentEvent.Set();
                }
                LogManager.Instance.Add(LogLevel.Medium, "Removed component {0} => {1}", Component.Name, rval);
                Component.Entity.Components.Remove(Component);
                Component.Entity.System.Deserialize();
                return true;
            }
            return false;
        }

        private void SetComponentEnabled(bool enabled)
        {
            Component.IsEnabled = enabled;
        }
    }
}
