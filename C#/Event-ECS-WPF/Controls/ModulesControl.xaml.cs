using Event_ECS_WPF.Projects.Love;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ModulesControl.xaml
    /// </summary>
    public partial class ModulesControl : UserControl
    {
        public ModulesControl()
        {
            InitializeComponent();
        }

        public Modules Modules
        {
            get { return (Modules)GetValue(ModulesProperty); }
            set { SetValue(ModulesProperty, value); }
        }

        public static readonly DependencyProperty ModulesProperty =
            DependencyProperty.Register("Modules", typeof(Modules), typeof(ModulesControl));
    }
}
