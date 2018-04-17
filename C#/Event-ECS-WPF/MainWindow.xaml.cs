using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Event_ECS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel m_viewmodel;

        public MainWindow()
        {
            InitializeComponent();
            m_viewmodel = (MainWindowViewModel)DataContext;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.Instance.Add(e.Exception.Message);
            e.Handled = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ECS.Instance?.Dispose();
            Settings.Default.Save();
        }
    }
}
