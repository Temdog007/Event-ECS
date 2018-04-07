using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

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

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(EntityComponentSystemControl));
    }
}
