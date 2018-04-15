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
    /// Interaction logic for Entity.xaml
    /// </summary>
    public partial class EntityControl : UserControl
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

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(EntityControl));

        public ICommand RemoveEntityCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand<object>(RemoveEntity));
        private ICommand m_removeEntityCommand;

        private int RemoveEntityFunc(ECSWrapper ecs)
        {
            return ecs.RemoveEntity(Entity.ID);
        }

        private void RemoveEntity(object param)
        {
            if(ECS.Instance != null)
            {
                ECS.Instance.UseWrapper(RemoveEntityFunc, out int removed);
                LogManager.Instance.Add("{0} entities removed", removed);
            }
        }

        public ICommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<object>(AddComponent));
        private ICommand m_addComponentCommand;

        private void AddComponentFunc(ECSWrapper ecs)
        {
            ecs.AddComponent(Entity.ID, Entity.System.SelectedComponent);
        }

        private void AddComponent(object param)
        {
            if (!string.IsNullOrWhiteSpace(Entity.System.SelectedComponent) && ECS.Instance != null)
            {
                ECS.Instance.UseWrapper(AddComponentFunc);
                Entity.System.Deserialize();
            }
        }
    }
}
