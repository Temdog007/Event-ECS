using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ComponentVariableControl.xaml
    /// </summary>
    public partial class ComponentVariableControl : UserControl
    {
        public ComponentVariableControl()
        {
            InitializeComponent();
        }

        public ComponentVariable ComponentVariable
        {
            get { return (ComponentVariable)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }

        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("ComponentVariable", typeof(ComponentVariable), typeof(ComponentVariableControl));
    }
}
