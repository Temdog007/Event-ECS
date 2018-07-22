using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using Event_ECS_WPF.SystemObjects.Communication;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for SystemControl.xaml
    /// </summary>
    public partial class EntityComponentSystemControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(EntityComponentSystemControl));

        public static readonly DependencyProperty ProjectProperty =
           DependencyProperty.Register("Project", typeof(Project), typeof(EntityComponentSystemControl));

        private IActionCommand m_addEntityCommand;
        
        private ActionCommand<string> m_dispatchEventCommand;

        private ActionCommand<string> m_broadcastEventCommand;

        private ActionCommand m_executeCodeCommand;

        public EntityComponentSystemControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand(AddEntity));
        
        public IActionCommand DispatchEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(DispatchEvent));

        public IActionCommand BroadcastEventCommand => m_broadcastEventCommand ?? (m_broadcastEventCommand = new ActionCommand<string>(BroadcastEvent));

        public IActionCommand ExecuteCodeCommand => m_executeCodeCommand ?? (m_executeCodeCommand = new ActionCommand(ExecuteCode));

        private void ExecuteCode()
        {
            using (var dialog = new Forms.OpenFileDialog
            {
                Filter = string.Format(MainWindowViewModel.DefaultFilterFormat, "lua")
            })
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    string code = File.ReadAllText(dialog.FileName);
                    ECS.Instance.Execute(code);
                }
            }
        }

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddEntity()
        {
            ECS.Instance.AddEntity(EntityComponentSystem.ID);
            LogManager.Instance.Add("Added entity");
        }

        private void DispatchEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.DispatchEvent(EntityComponentSystem.ID, ev);
        }

        private void BroadcastEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.BroadcastEvent(ev);
        }

        private void System_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEntityCommand.UpdateCanExecute(this, e);
        }

        private void EventText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DispatchEventCommand.UpdateCanExecute(sender, e);
            BroadcastEventCommand.UpdateCanExecute(sender, e);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scv)
            {
                scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }
    }
}
