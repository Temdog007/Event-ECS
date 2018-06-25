using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using Event_ECS_WPF.SystemObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{

    [XmlInclude(typeof(LoveProject))]
    [XmlRoot("Project")]
    public class Project : NotifyPropertyChanged
    {
        // Components that are compiled in the lua code
        public static readonly string[] DefaultComponents = 
        {
            "ColorComponent",
            "FinalizerComponent"
        };

        private ObservableCollection<ValueContainer<string>> _componentPath;

        private string _name;

        private ObservableCollection<ValueContainer<string>> _startupScripts;

        private ObservableCollection<ValueContainer<string>> m_libraryPaths;

        public Project() : this(false) { }

        public Project(bool setDefaults)
        {
            Name = "New Project";
            if(setDefaults)
            {
                ComponentPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                LibraryPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
        }

        public static string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string _outputPath = Location;

        [XmlElement]
        public string OutputPath
        {
            get => _outputPath;
            set
            {
                _outputPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Directory containing all of the lua component files
        /// </summary>
        [XmlArray("Component Directories")]
        [XmlArrayItem("Component Directory")]
        public ObservableCollection<ValueContainer<string>> ComponentPaths
        {
            get => _componentPath ?? (_componentPath = new ObservableCollection<ValueContainer<string>>());
            set
            {
                _componentPath = value;
                OnPropertyChanged("ComponentPaths");
                OnPropertyChanged("Components");
            }
        }

        public IEnumerable<string> Components
        {
            get
            {
                foreach (var comp in DefaultComponents)
                {
                    yield return comp;
                }
                foreach (var file in Files)
                {
                    yield return Path.GetFileNameWithoutExtension(Path.GetFileName(file));
                }
            }
        }

        public IEnumerable<string> Files
        {
            get
            {
                foreach (var componentPath in ComponentPaths)
                {
                    if (!IsHidden(componentPath) && Directory.Exists(componentPath))
                    {
                        foreach (var file in Directory.GetFiles(componentPath).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".lua"))
                        {
                            yield return file;
                        }
                    }
                }
            }
        }

        [XmlArray("Library Directories")]
        [XmlArrayItem("Library Directory")]
        public ObservableCollection<ValueContainer<string>> LibraryPaths
        {
            get => m_libraryPaths ?? (m_libraryPaths = new ObservableCollection<ValueContainer<string>>());
            set
            {
                m_libraryPaths = value;
                OnPropertyChanged("LibraryPaths");
            }
        }

        [XmlAttribute]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [XmlIgnore]
        public virtual ProcessStartInfo StartInfo
        {
            get
            {
                return new ProcessStartInfo()
                {
                    FileName = Properties.Settings.Default.Lua,
#if DEBUG
                    Arguments = string.Join(" ", string.Format("\"{0}\"", OutputPath), "DEBUG_MODE")
#else
                    Arguments = string.Join(" ", string.Format("\"{0}\"", OutputPath))
#endif
                };
            }
        }

        [XmlArray("Startup Script Directories")]
        [XmlArrayItem("Startup Script Directory")]
        public ObservableCollection<ValueContainer<string>> StartupScriptsPath
        {
            get => _startupScripts ?? (_startupScripts = new ObservableCollection<ValueContainer<string>>());
            set
            {
                _startupScripts = value;
                OnPropertyChanged("StartupScriptsPath");
            }
        }

        public virtual ProjectType Type
        {
            get => ProjectType.NORMAL;
        }
        
        public void CopyComponentsToOutputPath(bool reload = true)
        {
            foreach (string file in Files)
            {
                string dest = Path.Combine(OutputPath, Path.GetFileName(file));
                if (!File.Exists(dest) || File.GetLastWriteTimeUtc(dest) != (File.GetLastWriteTimeUtc(file)))
                {
                    File.Copy(file, dest, true);
                    var now = DateTime.Now;
                    File.SetLastWriteTimeUtc(file, now);
                    File.SetLastWriteTimeUtc(dest, now);
                    LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                    if (reload)
                    {
                        string modName = Path.GetFileNameWithoutExtension(file);
                        ECS.Instance.ReloadModule(modName);
                        LogManager.Instance.Add(LogLevel.Medium, "Reload {0} from cache", modName);
                    }
                }
            }
        }

        public virtual bool Start()
        {
            if (Setup())
            {
                File.WriteAllText(Path.Combine(OutputPath, "systemList.lua"), "Event_ECS_WPF.Lua.systemList.lua".GetResourceFileContents());
                File.WriteAllText(Path.Combine(OutputPath, "serverSystem.lua"), "Event_ECS_WPF.Lua.serverSystem.lua".GetResourceFileContents());
                StartApplication();
                return true;
            }
            return false;
        }

        private static bool IsHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }

        private void ExecuteInitialCode()
        {
            foreach (var scriptDir in StartupScriptsPath)
            {
                foreach (var script in Directory.GetFiles(scriptDir).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".lua"))
                {
                    string code = File.ReadAllText(script);
                    ECS.Instance.Execute(code);
                }
            }
        }

        private bool Setup()
        {
            try
            {
                foreach(var libraryPath in LibraryPaths)
                {
                    if (!IsHidden(libraryPath) && Directory.Exists(libraryPath))
                    {
                        foreach (var file in Directory.GetFiles(libraryPath).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".dll"))
                        {
                            string dest = Path.Combine(OutputPath, Path.GetFileName(file));
                            if (!File.Exists(dest))
                            {
                                File.Copy(file, dest);
                                LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                            }
                        }
                    }
                }

                CopyComponentsToOutputPath();

                ExecuteInitialCode();
                return true;
            }
            catch (Exception e)
            {
                ECS.Instance.Dispose();
                LogManager.Instance.Add(e);
                return false;
            }
        }

        protected virtual void StartApplication()
        {
            throw new NotImplementedException("Start application is not applicable for a generic project");
        }
    }
}
