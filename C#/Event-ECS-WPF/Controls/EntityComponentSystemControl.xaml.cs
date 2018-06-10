using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Timers;
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

        private IActionCommand m_serializeCommand;
        private ActionCommand m_executeCodeCommand;

        public EntityComponentSystemControl()
        {
            InitializeComponent();

            ECS.DeserializeRequested += ECS_DeserializeRequested;

            Timer.Elapsed += Timer_Elapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand(AddEntity));

        public bool AutoUpdate
        {
            get => Timer.Enabled;
            set
            {
                Timer.Enabled = value;
                OnPropertyChanged("AutoUpdate");
            }
        }
        
        public IActionCommand DispatchEventCommand => m_dispatchEventCommand ?? (m_dispatchEventCommand = new ActionCommand<string>(DispatchEvent));

        public IActionCommand BroadcastEventCommand => m_broadcastEventCommand ?? (m_broadcastEventCommand = new ActionCommand<string>(BroadcastEvent));

        public IActionCommand ExecuteCodeCommand => m_executeCodeCommand ?? (m_executeCodeCommand = new ActionCommand(ExecuteCode));

        private void ExecuteCodeFunc(ECSWrapper ecs, string code)
        {
            ecs.Execute(code, EntityComponentSystem.Name);
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

        public string ClassName => ECS.Instance.UseWrapper(GetClassName, out string classname) ? classname : EntityComponentSystem.Name;

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

        public IActionCommand SerializeCommand => m_serializeCommand ?? (m_serializeCommand = new ActionCommand(Serialize));

        public System.Timers.Timer Timer { get; } = new System.Timers.Timer
        {
            AutoReset = true,
            Enabled = true,
            Interval = Settings.Default.RefreshRate
        };

        public double UpdateInterval
        {
            get => Timer.Interval;
            set
            {
                Timer.Interval = value;
                Settings.Default.RefreshRate = Convert.ToUInt32(value);
                OnPropertyChanged("UpdateInterval");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddEntity()
        {
            if (ECS.Instance.UseWrapper(AddEntityFunc, out string str))
            {
                LogManager.Instance.Add("Added entity: {0}", str);
                EntityComponentSystem.Deserialize();
            }
        }

        private string AddEntityFunc(ECSWrapper ecs)
        {
            return ecs.AddEntity(EntityComponentSystem.Name);
        }
        
        private void DispatchEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.UseWrapper(ecs => ecs.DispatchEvent(EntityComponentSystem.Name, ev));
        }

        private void BroadcastEvent(string ev)
        {
            if (!ev.StartsWith("event", StringComparison.OrdinalIgnoreCase))
            {
                ev = "event" + ev;
            }
            ECS.Instance.UseWrapper(ecs => ecs.BroadcastEvent(ev));
        }

        private void Deserialize()
        {
            Dispatcher.BeginInvoke(new Action(() => EntityComponentSystem?.Deserialize()));
        }

        private void ECS_DeserializeRequested()
        {
            Deserialize();
        }

        private string GetClassName(ECSWrapper ecs)
        {
            return EntityComponentSystem == null ? string.Empty : ecs.GetClassName(EntityComponentSystem.Name);
        }

        private bool RemoveEntityFunc(ECSWrapper ecs, int entityID)
        {
            try
            {
                return ecs.RemoveEntity(EntityComponentSystem.Name, entityID);
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, "Cannot remove entity: {0}", e.Message);
                return false;
            }
        }

        private void Serialize()
        {
            OnPropertyChanged("ClassName");
            if (ECS.Instance.UseWrapper(ecs => ecs.SerializeSystem(EntityComponentSystem.Name), out string data))
            {
                EntityComponentSystem.Deserialize(data.Split('\n'));
                LogManager.Instance.Add(data, LogLevel.Low);
            }
        }

        private void System_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEntityCommand.UpdateCanExecute(this, e);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Deserialize();
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
