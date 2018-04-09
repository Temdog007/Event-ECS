using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System.Windows;

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

        private void m_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ECS.Instance.Dispose();
            Settings.Default.Save();
        }
    }
}
