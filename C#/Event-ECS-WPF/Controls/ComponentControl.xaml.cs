using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

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
    }
}
