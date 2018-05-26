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
        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("ComponentVariable", typeof(IComponentVariable), typeof(ComponentVariableControl));

        public ComponentVariableControl()
        {
            InitializeComponent();
        }

        public IComponentVariable ComponentVariable
        {
            get { return (IComponentVariable)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }
    }
}
