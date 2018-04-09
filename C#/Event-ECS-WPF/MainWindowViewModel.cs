using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public const string FileFilter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        public ActionCommand<Window> m_closeCommand;
        public ActionCommand<ProjectType> m_newProjectCommand;
        public ActionCommand<Window> m_openProjectCommand;
        public ActionCommand<Window> m_saveProjectCommand;
        public ActionCommand<Window> m_startProjectCommand;
        public ActionCommand<Window> m_stopProjectCommand;

        private string m_arguments = string.Empty;

        private ActionCommand<object> m_clearLogCommand;

        private Project m_project;

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
        public bool HasProject => Project != null;
        public bool IsLoveProject => Project is LoveProject;

        public ActionCommand<ProjectType> NewProjectCommand => m_newProjectCommand ?? (m_newProjectCommand = new ActionCommand<ProjectType>(NewProject));
        public ActionCommand<Window> OpenProjectCommand => m_openProjectCommand ?? (m_openProjectCommand = new ActionCommand<Window>(OpenProject));
        public Project Project
        {
            get => m_project;
            set
            {
                if (m_project != value)
                {
                    m_project = value;
                    OnPropertyChanged("Project");
                    OnPropertyChanged("HasProject");
                    OnPropertyChanged("IsLoveProject");
                }
            }
        }

        public ActionCommand<Window> SaveProjectCommand => m_saveProjectCommand ?? (m_saveProjectCommand = new ActionCommand<Window>(SaveProject));

        public ActionCommand<Window> StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand<Window>(StartProject));
        public ActionCommand<Window> StopProjectCommand => m_stopProjectCommand ?? (m_stopProjectCommand = new ActionCommand<Window>(StopProject));

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
                                LogManager.Instance.Add(new Log()
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
                        LogManager.Instance.Add(new Log()
                        {
                            DateTime = DateTime.Now,
                            Message = message
                        });
                    }
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        

        private void addLog(string message, params object[] args)
        {
            addLog(string.Format(message, args));
        }

        private void clearLogs()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LogManager.Instance.Clear();
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void NewProject(ProjectType type)
        {
            Project = type.CreateProject();
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
            StopProject();

            try
            {
                Project.Start();
                ECS.Instance.Create(Project);
            }
            catch(Exception e)
            {
                StopProject();
                LogManager.Instance.Add(e.Message);
            }

            OnPropertyChanged("ProjectStarted");
        }

        private void StopProject()
        {
            ECS.Instance.Dispose();
        }
    }
}
