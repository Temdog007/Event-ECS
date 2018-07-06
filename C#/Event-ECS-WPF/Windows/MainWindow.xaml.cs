using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Event_ECS_WPF.Windows
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
            if (e.InnerException != null)
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
            ECS.Instance.Dispose();
            Settings.Default.Save();
        }
        

        private void UpdateCommands(object sender, EventArgs e)
        {
            viewmodel.OpenRecentProjectCommand.UpdateCanExecute(sender, e);
            viewmodel.OpenProjectAtIndexCommand.UpdateCanExecute(sender, e);
        }

        private void UpdateCommands(object sender, MouseButtonEventArgs e)
        {
            UpdateCommands(sender, (EventArgs)e);
        }
    }
}
