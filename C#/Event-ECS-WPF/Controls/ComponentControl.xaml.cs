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
        public ComponentControl()
        {
            InitializeComponent();
        }

        public Component Component
        {
            get { return (Component)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }

        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("Component", typeof(Component), typeof(ComponentControl));

        public ICommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand<object>(RemoveComponent));
        private ICommand m_removeComponentCommand;

        private bool RemoveComponentFunc(ECSWrapper ecs)
        {
            object id = Component["id"];
            if (id != null)
            {
                return ecs.RemoveComponent(Component.Entity.ID, Convert.ToInt32(id));
            }
            return false;
        }

        private void RemoveComponent(object param)
        {
            if(ECS.Instance != null && ECS.Instance.UseWrapper(RemoveComponentFunc, out bool result))
            {
                LogManager.Instance.Add("Component removed: {0}", result);
                Component.Entity.System.Deserialize();
            }
        }
    }
}
