using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
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

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ComponentControl));

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

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public ICommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand<object>(RemoveComponent));
        public ICommand SetComponentEnabledCommand => m_setComponentEnabledCommand ?? (m_setComponentEnabledCommand = new ActionCommand<bool>(SetComponentEnabled));

        private void RemoveComponent(object param)
        {
            try
            {
                if (ECS.Instance.UseWrapper(RemoveComponentFunc, out bool result))
                {
                    LogManager.Instance.Add("Component removed: {0}", result);
                    Component.Entity.System.Deserialize();
                }
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, "Couldn't remove component: {0}\n{1}", Component.Name, e.Message);
            }
        }

        private void SetComponentEnabled(bool enabled)
        {
            Component.IsEnabled = enabled;
        }

        private bool RemoveComponentFunc(ECSWrapper ecs)
        {
           return ecs.RemoveComponent(Component.Entity.ID, Component.ID);
        }
    }
}
