using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Event_ECS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void m_window_Closing(object sender, CancelEventArgs e)
        {
            ECS.Instance?.Dispose();
            Settings.Default.Save();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Settings.Default.ManualUpdateKey.Convert())
            {
                ECS.Instance?.Update();
            }
        }
    }
}
