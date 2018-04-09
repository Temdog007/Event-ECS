using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using EventECSWrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Forms = System.Windows.Forms;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;
using System.Xml.Serialization;
using System.IO;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        public const string FileFilter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        public ActionCommand<Window> m_closeCommand;
        public ActionCommand<Window> m_newProjectCommand;
        public ActionCommand<Window> m_openProjectCommand;
        public ActionCommand<Window> m_saveProjectCommand;
        public ActionCommand<Window> m_startProjectCommand;
        private ECSWrapper ecs;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
        #endregion

        private string m_arguments = string.Empty;

        private ActionCommand<object> m_clearLogCommand;

        private object m_logLock = new object();

        private ObservableCollection<Log> m_logs = new ObservableCollection<Log>();

        private Project m_project;

        private ProjectType m_projectType = ProjectType.NORMAL;

        private bool m_showProject = false;

        private ECSSystem m_system = new ECSSystem();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Arguments
        {
            get => m_arguments;
            set
            {
                m_arguments = value;
                OnPropertyChanged("Arguments");
            }
        }

        public ActionCommand<object> ClearLogCommand => m_clearLogCommand ?? (m_clearLogCommand = new ActionCommand<object>(clearLogs));

        public ActionCommand<Window> CloseCommand => m_closeCommand ?? (m_closeCommand = new ActionCommand<Window>(CloseWindow));

        public ObservableCollection<Log> Logs
        {
            get => m_logs;
            set
            {
                m_logs = value;
                OnPropertyChanged("Logs");
            }
        }

        public ActionCommand<Window> NewProjectCommand => m_newProjectCommand ?? (m_newProjectCommand = new ActionCommand<Window>(NewProject));

        public ActionCommand<Window> OpenProjectCommand => m_openProjectCommand ?? (m_openProjectCommand = new ActionCommand<Window>(OpenProject));

        public Project Project
        {
            get => m_project;
            set
            {
                if (m_project != value)
                {
                    m_project = value;
                    ShowProject = true;
                    OnPropertyChanged("Project");
                    OnPropertyChanged("HasProject");
                }
            }
        }

        public bool ProjectedStarted => ecs != null;

        public bool HasProject => Project != null;

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
        }

        public ActionCommand<Window> SaveProjectCommand => m_saveProjectCommand ?? (m_saveProjectCommand = new ActionCommand<Window>(SaveProject));

        public bool ShowProject
        {
            get => m_showProject;
            set
            {
                m_showProject = value;
                OnPropertyChanged("ShowProject");
            }
        }

        public ActionCommand<Window> StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand<Window>(StartProject));

        public ECSSystem System
        {
            get => m_system;
            set
            {
                m_system = value;
                OnPropertyChanged("System");
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

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
                            foreach (var line in message.Split('\n'))
                            {
                                foreach (string str in line.Split(Settings.Default.MaxLogLength))
                                {
                                    Logs.Add(new Log()
                                    {
                                        DateTime = addDate ? DateTime.Now : default(DateTime?),
                                        Message = str
                                    });
                                    addDate = false;
                                }
                            }
                        }
                        else
                        {
                            Logs.Add(new Log()
                            {
                                DateTime = DateTime.Now,
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
        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void NewProject()
        {
            Project = ProjectType.CreateProject();
        }

        private void OpenProject()
        {
            using (var dialog = new Forms.OpenFileDialog())
            {
                dialog.Filter = FileFilter;
                dialog.RestoreDirectory = true;
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        using (Stream s = dialog.OpenFile())
                        {
                            var serializer = new XmlSerializer(typeof(Project));
                            Project = (Project)serializer.Deserialize(s);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveProject()
        {
            using (var dialog = new Forms.SaveFileDialog())
            {
                dialog.Filter = FileFilter;
                dialog.RestoreDirectory = true;
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        using (Stream s = dialog.OpenFile())
                        {
                            var serializer = new XmlSerializer(typeof(Project));
                            serializer.Serialize(s, Project);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void StartProject()
        {
            ecs?.Dispose();

            try
            {
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
    }
}
