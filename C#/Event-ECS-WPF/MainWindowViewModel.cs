using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public const string FileFilter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private ActionCommand<Window> m_closeCommand;
        private ActionCommand<ProjectType> m_newProjectCommand;
        private ActionCommand m_openProjectCommand;
        private ActionCommand m_saveProjectCommand;
        private ActionCommand m_startProjectCommand;
        private ActionCommand m_stopProjectCommand;
        private ActionCommand m_toggleProjectCommand;
        private ActionCommand m_toggleProjectModeCommand;
        private ActionCommand m_manualUpdateCommand;

        private string m_arguments = string.Empty;

        private ICommand m_clearLogCommand;
        
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
        public ICommand ClearLogCommand => m_clearLogCommand ?? (m_clearLogCommand = new ActionCommand(ClearLogs));
        public ActionCommand<Window> CloseCommand => m_closeCommand ?? (m_closeCommand = new ActionCommand<Window>(CloseWindow));
        public bool HasProject => Project != null;
        public ActionCommand<ProjectType> NewProjectCommand => m_newProjectCommand ?? (m_newProjectCommand = new ActionCommand<ProjectType>(NewProject));
        public ICommand OpenProjectCommand => m_openProjectCommand ?? (m_openProjectCommand = new ActionCommand(OpenProject));
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
                }
            }
        }

        public ICommand SaveProjectCommand => m_saveProjectCommand ?? (m_saveProjectCommand = new ActionCommand(SaveProject));
        public ICommand StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand(StartProject));
        public ICommand StopProjectCommand => m_stopProjectCommand ?? (m_stopProjectCommand = new ActionCommand(StopProject));
        public ICommand ToggleProjectCommand => m_toggleProjectCommand ?? (m_toggleProjectCommand = new ActionCommand(ToggleProject));
        public ICommand ToggleProjectModeCommand => m_toggleProjectModeCommand ?? (m_toggleProjectModeCommand = new ActionCommand(ToggleProjectMode));
        public ICommand ManualUpdateCommand => m_manualUpdateCommand ?? (m_manualUpdateCommand = new ActionCommand(ManualUpdate));

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

        private void ClearLogs()
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
                System.Deserialize();
            }
            catch(Exception e)
            {
                StopProject();
                LogManager.Instance.Add(LogLevel.High, e.Message);
            }
        }

        private void StopProject()
        {
            Project.Stop();
        }

        private void ToggleProject()
        {
            if(Project != null)
            {
                if(Project.IsStarted)
                {
                    StopProject();
                }
                else
                {
                    StartProject();
                }
            }
        }

        private void ToggleProjectMode()
        {
            ECS.Instance?.SetAutoUpdate(!ECS.Instance.GetAutoUpdate());
        }

        private void ManualUpdate()
        {
            ECS.Instance?.Update();
        }
    }
}
