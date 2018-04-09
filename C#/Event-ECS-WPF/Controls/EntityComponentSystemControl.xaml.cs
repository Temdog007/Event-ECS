using Event_ECS_WPF.Commands;
using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for SystemControl.xaml
    /// </summary>
    public partial class EntityComponentSystemControl : UserControl
    {
        public EntityComponentSystemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(EntityComponentSystemControl));

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public ICommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand<object>(AddEntity));
        private ICommand m_addEntityCommand;

        private void AddEntity(object param)
        {
            new Entity(EntityComponentSystem);
        }
    }
}
