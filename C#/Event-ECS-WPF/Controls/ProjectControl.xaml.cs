using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private ICommand m_serializeCommand;

        public ProjectControl()
        {
            InitializeComponent();
        }

        public ICommand DispatchEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(DispatchEvent));

        public ICommand GetPathCommand => m_getPathCommand ?? (m_getPathCommand = new ActionCommand<string>(GetPath));

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }
        public ICommand SerializeCommand => m_serializeCommand ?? (m_serializeCommand = new ActionCommand(Serialize));

        private void DispatchEvent(string ev)
        {
            if (ECS.Instance != null && ECS.Instance.UseWrapper(ecs => ecs.DispatchEvent(ev), out int handles))
            {
                LogManager.Instance.Add(LogLevel.Medium, "Event '{0}' was handled '{1}' time(s)", ev, handles);
            }
        }

        private void eventText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            TextBox box = sender as TextBox;
            if (box != null)
            {
                DispatchEvent(box.Text);
                e.Handled = true;
            }
        }

        private void GetPath(string propertyName)
        {
            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                switch(dialog.ShowDialog())
                {
                    case System.Windows.Forms.DialogResult.OK:
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
        private void Serialize()
        {
            if (ECS.Instance != null && ECS.Instance.UseWrapper(ecs => ecs.Serialize(), out string data))
            {
                EntityComponentSystem.Instance.Deserialize(data.Split('\n'));
                LogManager.Instance.Add(data, LogLevel.Low);
            }
        }
    }
}
