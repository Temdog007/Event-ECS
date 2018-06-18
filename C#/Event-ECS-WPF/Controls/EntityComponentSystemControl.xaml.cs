using Event_ECS_Lib;
using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
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

        private ActionCommand<string> m_broadcastEventCommand;

        private string m_classname = string.Empty;

        private ActionCommand<string> m_dispatchEventCommand;

        private ActionCommand m_executeCodeCommand;

        private IActionCommand m_serializeCommand;

        public EntityComponentSystemControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand(AddEntity));

        public IActionCommand BroadcastEventCommand => m_broadcastEventCommand ?? (m_broadcastEventCommand = new ActionCommand<string>(BroadcastEvent));

        public string ClassName
        {
            get => m_classname;
            set
            {
                m_classname = value ?? string.Empty;
                OnPropertyChanged("ClassName");
            }
        }

        public IActionCommand DispatchEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(DispatchEvent));

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public IActionCommand ExecuteCodeCommand => m_executeCodeCommand ?? (m_executeCodeCommand = new ActionCommand(ExecuteCode));

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public IActionCommand SerializeCommand => m_serializeCommand ?? (m_serializeCommand = new ActionCommand(Serialize));

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddEntity()
        {
            ECS.Instance.UseWrapper(AddEntityFunc, AddEntityResponse);
        }

        private void AddEntityFunc(IECSWrapper ecs)
        {
            ecs.AddEntity(EntityComponentSystem.Name);
        }

        private bool AddEntityResponse(string func, object result)
        {
            if (func == "AddEntity")
            {
                LogManager.Instance.Add("Added entity: {0}", result);
                EntityComponentSystem.Deserialize();
                return true;
            }
            return false;
        }

        private void BroadcastEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.UseWrapper(ecs => ecs.BroadcastEvent(ev));
        }

        private bool ClassNameResponse(string funcName, object result)
        {
            if (funcName == "GetClassName")
            {
                ClassName = result as string;
                return true;
            }
            return false;
        }

        private void Deserialize()
        {
            Dispatcher.BeginInvoke(new Action(() => EntityComponentSystem?.Deserialize()));
        }

        private void DispatchEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.UseWrapper(ecs => ecs.DispatchEvent(EntityComponentSystem.Name, ev));
        }

        private void EventText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DispatchEventCommand.UpdateCanExecute(sender, e);
            BroadcastEventCommand.UpdateCanExecute(sender, e);
        }

        private void ExecuteCode()
        {
            var dialog = new Forms.OpenFileDialog
            {
                Filter = string.Format(MainWindowViewModel.DefaultFilterFormat, "lua")
            };
            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                string code = File.ReadAllText(dialog.FileName);
                ECS.Instance.UseWrapper(ExecuteCodeFunc, code);
            }
        }

        private void ExecuteCodeFunc(IECSWrapper ecs, string code)
        {
            ecs.Execute(code, EntityComponentSystem.Name);
        }

        private void GetClassName(IECSWrapper ecs)
        {
            if (EntityComponentSystem != null)
            {
                ecs.GetClassName(EntityComponentSystem.Name);
            }
        }
        
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scv)
            {
                scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void Serialize()
        {
            ECS.Instance.UseWrapper(GetClassName, ClassNameResponse);
            ECS.Instance.UseWrapper(ecs => ecs.SerializeSystem(EntityComponentSystem.Name), SerializeResponse);
        }

        private bool SerializeResponse(string funcName, object result)
        {
            if(funcName == "Serialize" && result is string data)
            {
                EntityComponentSystem.Deserialize(data.Split('\n'));
                LogManager.Instance.Add(data, LogLevel.Low);
                return true;
            }
            return false;
        }

        private void System_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEntityCommand.UpdateCanExecute(this, e);
        }
    }
}
