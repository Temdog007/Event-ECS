﻿using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public const string ComponentFormat =
@"local Component = require('component')
local class = require('classlib')

local {0} = class('{0}Component', Component)

function {0}:__init(entity)
  self.Component:__init(entity, self)
end

return {0}";

        public const string DefaultFilterFormat = "{0} files (*.{0})|*.{0}|All files (*.*)|*.*";
        private string m_arguments = string.Empty;

        private ICommand m_clearLogCommand;

        private ActionCommand<Window> m_closeCommand;

        private ICommand m_createComponentCommand;

        private ECSSystem m_currentSystem;

        private ICommand m_editComponentCommand;

        private ActionCommand m_manualUpdateCommand;

        private ActionCommand<ProjectType> m_newProjectCommand;

        private ActionCommand m_openProjectCommand;

        private Project m_project;

        private ActionCommand m_saveProjectCommand;

        private ICommand m_setComponentSettingsCommand;

        private ActionCommand m_startProjectCommand;

        private ActionCommand m_stopProjectCommand;

        private ObservableCollection<ECSSystem> m_systems = new ObservableCollection<ECSSystem>();

        private ActionCommand m_toggleProjectCommand;

        private ActionCommand m_toggleProjectModeCommand;

        public MainWindowViewModel()
        {
            Project.ProjectStateChange += ECS_OnAutoUpdateChanged;
            ECS.OnAutoUpdateChanged += ECS_OnAutoUpdateChanged;
        }

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

        public ICommand CreateComponentCommand => m_createComponentCommand ?? (m_createComponentCommand = new ActionCommand(CreateComponent));

        public ECSSystem CurrentSystem
        {
            get => m_currentSystem;
            set
            {
                m_currentSystem = value;
                OnPropertyChanged("CurrentSystem");
            }
        }

        public ICommand EditComponentCommand => m_editComponentCommand ?? (m_editComponentCommand = new ActionCommand(EditComponent));

        public bool HasProject => Project != null;

        public ICommand ManualUpdateCommand => m_manualUpdateCommand ?? (m_manualUpdateCommand = new ActionCommand(ManualUpdate));

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
                    OnPropertyChanged("ProjectBackground");
                }
            }
        }

        public SolidColorBrush ProjectBackground
        {
            get
            {
                if (Project == null)
                {
                    return Brushes.Red;
                }
                switch (Project.Type)
                {
                    case ProjectType.LOVE:
                        if (Project.IsStarted)
                        {
                            return ECS.Instance.GetAutoUpdate() ? Brushes.Lime : Brushes.LightYellow;
                        }
                        else
                        {
                            return Brushes.LightPink;
                        }
                    default:
                        return Project.IsStarted ? Brushes.Lime : Brushes.LightPink;
                }
            }
        }

        public ICommand SaveProjectCommand => m_saveProjectCommand ?? (m_saveProjectCommand = new ActionCommand(SaveProject));

        public ICommand SetComponentSettingsCommand => m_setComponentSettingsCommand ?? (m_setComponentSettingsCommand = new ActionCommand(SetComponentSettings));

        public ICommand StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand(StartProject));

        public ICommand StopProjectCommand => m_stopProjectCommand ?? (m_stopProjectCommand = new ActionCommand(StopProject));

        public ObservableCollection<ECSSystem> Systems
        {
            get => m_systems;
            set
            {
                m_systems = value;
                OnPropertyChanged("Systems");
            }
        }

        public ICommand ToggleProjectCommand => m_toggleProjectCommand ?? (m_toggleProjectCommand = new ActionCommand(ToggleProject));

        public ICommand ToggleProjectModeCommand => m_toggleProjectModeCommand ?? (m_toggleProjectModeCommand = new ActionCommand(ToggleProjectMode));

        public static string GetFileFilter(string ext)
        {
            return string.Format(DefaultFilterFormat, ext);
        }
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ClearLogs()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() => LogManager.Instance.Clear()));
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

        private void CreateComponent()
        {
            try
            {
                using (Forms.SaveFileDialog dialog = new Forms.SaveFileDialog()
                {
                    Filter = GetFileFilter("lua")
                })
                {
                    if (dialog.ShowDialog() == Forms.DialogResult.OK)
                    {
                        string compName = Path.GetFileNameWithoutExtension(dialog.FileName);
                        if (string.IsNullOrWhiteSpace(compName) || char.IsDigit(compName[0]) || compName.Any(c => !char.IsLetterOrDigit(c)))
                        {
                            MessageBox.Show("Invalid component name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            File.WriteAllText(dialog.FileName, string.Format(ComponentFormat, compName));
                            Process.Start(Settings.Default.ComponentEditor, dialog.FileName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
            }
        }

        private void ECS_OnAutoUpdateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("ProjectBackground");
        }

        private void EditComponent()
        {
            try
            {
                using (Forms.OpenFileDialog dialog = new Forms.OpenFileDialog
                {
                    Filter = GetFileFilter("lua")
                })
                {
                    if (dialog.ShowDialog() == Forms.DialogResult.OK)
                    {
                        Process.Start(Settings.Default.ComponentEditor, dialog.FileName);
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
            }
        }

        private void ManualUpdate()
        {
            ECS.Instance.Update();
        }

        private void NewProject(ProjectType type)
        {
            Project = type.CreateProject();
        }

        private void OpenProject()
        {
            using (var dialog = new Forms.OpenFileDialog())
            {
                dialog.Filter = GetFileFilter("xml");
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
                dialog.Filter = GetFileFilter("xml");
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

        private string[] Serialize(ECSWrapper ecs)
        {
            return ecs.Serialize();
        }

        private void SetComponentSettings()
        {
            try
            {
                using (Forms.OpenFileDialog dialog = new Forms.OpenFileDialog
                {
                    Filter = GetFileFilter("exe"),
                    InitialDirectory = Path.GetDirectoryName(Settings.Default.ComponentEditor)
                })
                {
                    if (dialog.ShowDialog() == Forms.DialogResult.OK)
                    {
                        Settings.Default.ComponentEditor = dialog.FileName;
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
            }
        }

        private void StartProject()
        {
            StopProject();

            try
            {
                Project.Start();
                if(ECS.Instance.UseWrapper(Serialize, out string[] data))
                {
                    if (data.Length > 0)
                    {
                        foreach (string line in data)
                        {
                            Systems.Add(new ECSSystem(line.Split('\n')));
                        }
                        CurrentSystem = Systems[0];
                    }
                }
                OnPropertyChanged("ProjectBackground");
            }
            catch(Exception e)
            {
                StopProject();
                LogManager.Instance.Add(LogLevel.High, e.Message);
            }
        }

        private void StopProject()
        {
            Systems.Clear();
            CurrentSystem = null;
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
            ECS.Instance.SetAutoUpdate(!ECS.Instance.GetAutoUpdate());
        }
    }
}
