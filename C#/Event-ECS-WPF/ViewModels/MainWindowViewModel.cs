using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.Properties;
using Event_ECS_WPF.SystemObjects;
using Event_ECS_WPF.SystemObjects.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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

local {0} = class('{0}', Component)

function {0}:__init(entity)
  self.Component:__init(entity, self)
end

function {0}:eventUpdate(args)
end

function {0}:eventDraw(args)
end

return {0}";

        public const string DefaultFilterFormat = "{0} files (*.{0})|*.{0}|All files (*.*)|*.*";

        private static readonly string[] SystemDelim = new string[] { "System" };

        private ICommand m_clearLogCommand;

        private ActionCommand<Window> m_closeCommand;

        private IActionCommand m_copyComponentsCommand;

        private ICommand m_createComponentCommand;

        private ECSSystem m_currentSystem;

        private IActionCommand m_editComponentCommand;

        private ActionCommand<ProjectType> m_newProjectCommand;

        private ActionCommand<int> m_openProjectAtIndexCommand;

        private ActionCommand m_openProjectCommand;

        private ActionCommand<string> m_openRecentProjectCommand;

        private Project m_project;

        private IActionCommand m_saveProjectCommand;

        private ICommand m_setComponentSettingsCommand;

        private ActionCommand m_startProjectCommand;

        private ObservableCollection<ECSSystem> m_systems = new ObservableCollection<ECSSystem>();

        public MainWindowViewModel()
        {
            Settings.Default.SettingChanging += Default_SettingChanging;
            ECS.Instance.DataReceived += OnDataReceived;
            ECS.Instance.ServerDisconnect += ServerDisconnect;
        }

        private delegate void SettingsUpdateDelegate(object sender, EventArgs e);

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ClearLogCommand => m_clearLogCommand ?? (m_clearLogCommand = new ActionCommand(ClearLogs));

        public ActionCommand<Window> CloseCommand => m_closeCommand ?? (m_closeCommand = new ActionCommand<Window>(CloseWindow));

        public IActionCommand CopyComponentsCommand => m_copyComponentsCommand ?? (m_copyComponentsCommand = new ActionCommand(CopyComponents, () => HasProject));

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

        public IActionCommand EditComponentCommand => m_editComponentCommand ?? (m_editComponentCommand = new ActionCommand(EditComponent, () => Settings.Default.LoadComponentEditor));

        public bool HasProject => Project != null;

        public ActionCommand<ProjectType> NewProjectCommand => m_newProjectCommand ?? (m_newProjectCommand = new ActionCommand<ProjectType>(NewProject));

        public IActionCommand OpenProjectAtIndexCommand => m_openProjectAtIndexCommand ?? (m_openProjectAtIndexCommand = new ActionCommand<int>(OpenProject));

        public ICommand OpenProjectCommand => m_openProjectCommand ?? (m_openProjectCommand = new ActionCommand(OpenProject));

        public IActionCommand OpenRecentProjectCommand => m_openRecentProjectCommand ?? (m_openRecentProjectCommand = new ActionCommand<string>(OpenProject));

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
                    SaveProjectCommand.UpdateCanExecute(this, EventArgs.Empty);
                    CopyComponentsCommand.UpdateCanExecute(this, EventArgs.Empty);
                }
            }
        }

        public IActionCommand SaveProjectCommand => m_saveProjectCommand ?? (m_saveProjectCommand = new ActionCommand(SaveProject, () => HasProject));

        public ICommand SetComponentSettingsCommand => m_setComponentSettingsCommand ?? (m_setComponentSettingsCommand = new ActionCommand(SetComponentSettings));

        public ICommand StartProjectCommand => m_startProjectCommand ?? (m_startProjectCommand = new ActionCommand(StartProject));

        public ObservableCollection<ECSSystem> Systems
        {
            get => m_systems;
            set
            {
                m_systems = value;
                OnPropertyChanged("Systems");
            }
        }

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

        private void CopyComponents() => Project?.CopyComponentsToOutputPath();

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
                            if (Settings.Default.LoadComponentEditor)
                            {
                                Process.Start(Settings.Default.ComponentEditor, dialog.FileName);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
            }
        }

        private void Default_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            SettingsUpdateDelegate d = DoSettingsUpdate;
            Application.Current.Dispatcher.BeginInvoke(d, sender, e);
        }

        private void DoSettingsUpdate(object sender, EventArgs e)
        {
            EditComponentCommand.UpdateCanExecute(sender, e);
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
                    if (dialog.ShowDialog() == Forms.DialogResult.OK && Settings.Default.LoadComponentEditor)
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

        private void NewProject(ProjectType type)
        {
            Project = type.CreateProject();
        }

        private void OnDataReceived(string data)
        {
            List<string> dataString = new List<string>(data.Split('\n'));
            while(dataString.Count > 0)
            {
                string line = dataString.First();
                string name = line.Split('|').First();

                if (name == "system")
                {
                    ECS_Object.GetData(line, out ECSData nameData, out ECSData enabledData, out ECSData idData, out List<ECSData> dataList);
                    dataString.RemoveAt(0);

                    ECSSystem system = Systems.FirstOrDefault(s => s.Name == nameData.Value);

                    if (system == null)
                    {
                        Systems.Add(new ECSSystem(
                            nameData.Value,
                            Convert.ToBoolean(enabledData.Value),
                            Convert.ToInt32(idData.Value),
                            dataString
                        ));
                    }
                    else
                    {
                        system.Name = nameData.Value;
                        system.IsEnabled = Convert.ToBoolean(enabledData.Value);
                        system.ID = Convert.ToInt32(idData.Value);
                        system.Deserialize(dataString);
                    }
                }
                else
                {
                    LogManager.Instance.Add(line);
                    dataString.RemoveAt(0);
                }
            }
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
                        OpenProject(dialog.FileName);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OpenProject(string projectName)
        {
            using (Stream s = new FileStream(projectName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var serializer = new XmlSerializer(typeof(Project));
                Project = (Project)serializer.Deserialize(s);
                LogManager.Instance.Add("Loaded project: {0}", projectName);
            }
            UpdateRecentProjects(projectName);
        }

        private void OpenProject(int index)
        {
            if (index >= Settings.Default.RecentProjects.Count)
            {
                LogManager.Instance.Add(LogLevel.High, "Cannot open project at index: {0}", index);
                return;
            }

            try
            {
                OpenProject(Settings.Default.RecentProjects[index]);
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
                Settings.Default.RecentProjects.RemoveAt(index);
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
                        UpdateRecentProjects(dialog.FileName);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ServerDisconnect()
        {
            Application.Current?.Dispatcher.BeginInvoke(new Action(Systems.Clear));
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
            if (Project != null)
            {
                ECS.Instance.Reset();
                Systems.Clear();
                if (!Project.Start())
                {
                    throw new Exception("Failed to start project");
                }
            }
        }

        private void UpdateRecentProjects(string filename)
        {
            if (Settings.Default.RecentProjects == null)
            {
                Settings.Default.RecentProjects = new ObservableCollection<string>();
            }
            Settings.Default.RecentProjects.Insert(0, filename);
            Settings.Default.RecentProjects = new ObservableCollection<string>(Settings.Default.RecentProjects.Distinct());
            while (Settings.Default.RecentProjects.Count > 10)
            {
                Settings.Default.RecentProjects.RemoveAt(9);
            }
        }
    }
}
