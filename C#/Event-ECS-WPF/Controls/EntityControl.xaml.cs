using Event_ECS_WPF.SystemObjects;
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

        private void RemoveEntity(object param)
        {
            Entity.Dispose();
        }

        public ICommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<object>(AddComponent));
        private ICommand m_addComponentCommand;

        private void AddComponent(object param)
        {
            Entity.AddComponent();
        }
    }
}
