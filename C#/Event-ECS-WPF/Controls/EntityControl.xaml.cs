using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

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
    }
}
