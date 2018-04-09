using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for LoveProjectSettings.xaml
    /// </summary>
    public partial class LoveProjectSettingsControl : UserControl
    {
        public LoveProjectSettingsControl()
        {
            InitializeComponent();
        }

        public LoveProjectSettings Settings
        {
            get { return (LoveProjectSettings)GetValue(ProjectSettingsProperty); }
            set { SetValue(ProjectSettingsProperty, value); }
        }

        public static readonly DependencyProperty ProjectSettingsProperty =
            DependencyProperty.Register("Settings", typeof(LoveProjectSettings), typeof(LoveProjectSettingsControl));
    }
}
