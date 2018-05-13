using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System;
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

        private static void LogException(Exception e)
        {
            LogManager.Instance.Add(LogLevel.High, e.Message);
            if(e.InnerException != null)
            {
                LogException(e.InnerException);
            }
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
            e.Handled = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (m_viewmodel.Project?.IsStarted ?? false)
            {
                MessageBox.Show("Cannot close application while project is running!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                LogManager.Instance.Add("Cannot close application while project is running!", LogLevel.Medium);
                e.Cancel = true;
            }
            else
            {
                ECS.Instance.Dispose();
                Settings.Default.Save();
            }
        }
    }
}
