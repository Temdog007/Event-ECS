using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using EventECSWrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private ECSWrapper ecs;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ecs?.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public ECSSystem System
        {
            get => m_system;
            set
            {
                m_system = value;
                OnPropertyChanged("System");
            }
        }private ECSSystem m_system = new ECSSystem();

        public ObservableCollection<Log> Logs
        {
            get => m_logs;
            set
            {
                m_logs = value;
                OnPropertyChanged("Logs");
            }
        }
        private ObservableCollection<Log> m_logs = new ObservableCollection<Log>();

        private object m_logLock = new object();
        private void addLog(string message)
        {
            lock (m_logLock)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (Settings.Default.MultilineLog)
                        {
                            bool addDate = true;
                            foreach (string str in message.Split(Settings.Default.MaxLogLength))
                            {
                                Logs.Add(new Log()
                                {
                                    DateTime = addDate ? DateTime.Now : default(DateTime?),
                                    Message = str
                                });
                                addDate = false;
                            }
                        }
                        else
                        {
                            Logs.Add(new Log()
                            {
                                DateTime =DateTime.Now,
                                Message = message
                            });
                        }
                    }));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void addLog(string message, params object[] args)
        {
            addLog(string.Format(message, args));
        }

        private void clearLogs()
        {
            lock(m_logLock)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Logs.Clear();
                    }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public ActionCommand<object> ClearLogCommand => m_clearLogCommand ?? (m_clearLogCommand = new ActionCommand<object>(clearLogs));
        private ActionCommand<object> m_clearLogCommand;

        public ActionCommand<Window> CloseCommand => m_closeCommand ?? (m_closeCommand = new ActionCommand<Window>(CloseWindow));
        public ActionCommand<Window> m_closeCommand;

        public bool ProjectedStarted => ecs != null;

        public ActionCommand<Window> StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand<Window>(StartProject));
        public ActionCommand<Window> m_startProjectCommand;

        public ProjectType ProjectType
        {
            get => m_projectType;
            set
            {
                if (m_projectType != value)
                {
                    m_projectType = value;
                    Project = m_projectType.CreateProject();
                    OnPropertyChanged("ProjectType");
                }
            }
        } private ProjectType m_projectType = ProjectType.NORMAL;

        public Project Project
        {
            get => m_project;
            set
            {
                if(m_project != null)
                {
                    m_project = value;
                    OnPropertyChanged("Project");
                }
            }
        } private Project m_project;

        public bool ShowProject
        {
            get => m_showProject;
            set
            {
                m_showProject = value;
                OnPropertyChanged("ShowProject");
            }
        } private bool m_showProject = false;

        private void StartProject()
        {
            ecs?.Dispose();

            try
            {
                if(Project == null)
                {
                    Project = ProjectType.CreateProject();
                }

                ecs = new ECSWrapper();
                ecs.Initialize(Project.Name, Convert.ToInt32(Project.Type));
            }
            catch(Exception e)
            {
                addLog(e.Message);
                ecs?.Dispose();
                ecs = null;
            }

            OnPropertyChanged("ProjectStarted");
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        public string Arguments
        {
            get => m_arguments;
            set
            {
                m_arguments = value;
                OnPropertyChanged("Arguments");
            }
        } private string m_arguments = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
