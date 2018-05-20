using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectControl));

        private ActionCommand<string> m_dispatchEventCommand;

        private ActionCommand<string> m_getPathCommand;

        public ProjectControl()
        {
            InitializeComponent();
        }

        public IActionCommand BroadcastEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(BroadcastEvent));

        public IActionCommand GetPathCommand => m_getPathCommand ?? (m_getPathCommand = new ActionCommand<string>(GetPath));

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private void BroadcastEvent(string ev)
        {
            if(!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            if (ECS.Instance.UseWrapper(ecs => ecs.BroadcastEvent(ev), out int handles))
            {
                LogManager.Instance.Add(LogLevel.Medium, "Event '{0}' was handled '{1}' time(s)", ev, handles);
            }
        }

        private void EventText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                BroadcastEventCommand.UpdateCanExecute(this, EventArgs.Empty);
                return;
            }

            if (sender is TextBox box)
            {
                BroadcastEvent(box.Text);
                e.Handled = true;
            }
        }

        private void GetPath(string propertyName)
        {
            using(var dialog = new Forms.FolderBrowserDialog())
            {
                switch(dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        if(!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            Project.SetProperty(propertyName, dialog.SelectedPath);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog
            {
                Filter = "lua files (*.lua)|*.lua|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                Project.InitializerScript = dialog.FileName;
            }
        }
    }
}
