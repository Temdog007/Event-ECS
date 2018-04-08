using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public ICommand RemoveComponentCommand => m_removeComponentCommand ?? (m_removeComponentCommand = new ActionCommand<object>(RemoveComponent));
        private ICommand m_removeComponentCommand;

        private void RemoveComponent(object param)
        {
            Component.Dispose();
        }

        private void SetVisibility(ContentControl ctrl, Visibility visibility)
        {
            if (ctrl != null)
            {
                UIElement element = ctrl.Content as UIElement;
                if (element != null)
                {
                    element.Visibility = visibility;
                }
            }
        }

        private void GroupBox_MouseEnter(object sender, MouseEventArgs e)
        {
            SetVisibility(sender as ContentControl, Visibility.Visible);
        }

        private void GroupBox_MouseLeave(object sender, MouseEventArgs e)
        {
            SetVisibility(sender as ContentControl, Settings.Default.OnlyShowComponentWhenMouse ? Visibility.Collapsed : Visibility.Visible);
        }
    }
}
