using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for EntityVariableControl.xaml
    /// </summary>
    public partial class EntityVariableControl : UserControl
    {
        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("EntityVariable", typeof(IEntityVariable), typeof(EntityVariableControl));

        public EntityVariableControl()
        {
            InitializeComponent();
        }

        public IEntityVariable EntityVariable
        {
            get { return (IEntityVariable)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }
    }
}
