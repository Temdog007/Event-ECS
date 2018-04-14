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
        public ProjectControl()
        {
            InitializeComponent();
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectControl));

        public ICommand ButtonClickCommand => m_buttonClickCommand ?? (m_buttonClickCommand = new ActionCommand<string>(Button_Click));
        private ActionCommand<string> m_buttonClickCommand;

        private void Button_Click(string propertyName)
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

        public ICommand DispatchEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(DispatchEvent));
        private ActionCommand<string> m_dispatchEventCommand;

        private void DispatchEvent(string ev)
        {
            if (ECS.Instance != null)
            {
                ECS.Instance.UseWrapper(ecs => ecs.DispatchEvent(ev), out int handles);
                LogManager.Instance.Add(LogLevel.Medium, "Event '{0}' was handled '{1}' time(s)", ev, handles);
            }
        }

        public ICommand SerializeCommand => m_serializeCommand ?? (m_serializeCommand = new ActionCommand<object>(Serialize));
        private ActionCommand<object> m_serializeCommand;

        private void Serialize()
        {
            if (ECS.Instance != null)
            {
                ECS.Instance.UseWrapper(ecs => ecs.Serialize(), out string data);
                LogManager.Instance.Add(data, LogLevel.Low);
            }
        }

        private void eventText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter)
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
    }
}
