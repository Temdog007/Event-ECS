using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using Event_ECS_WPF.SystemObjects;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{
    public delegate void ProjectStateChangeEvent(object sender, ProjectStateChangeArgs args);

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

        private string _initializer;

        private string _name;

        private string m_libraryPath;

        public Project() : this(false) { }

        public Project(bool setDefaults)
        {
            Name = "New Project";
            if(setDefaults)
            {
                ComponentPaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            LibraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public static string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Directory containing all of the lua component files
        /// </summary>
        [XmlElement]
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

        [XmlElement]
        public string InitializerScript
        {
            get => _initializer;
            set
            {
                _initializer = value;
                OnPropertyChanged("InitializerScript");
            }
        }
        
        [XmlElement]
        public string LibraryPath
        {
            get => m_libraryPath;
            set
            {
                m_libraryPath = value;
                OnPropertyChanged("LibraryPath");
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

        public virtual ProjectType Type
        {
            get => ProjectType.NORMAL;
        }
        
        public void CopyComponentsToOutputPath(bool reload = true)
        {
            foreach (string file in Files)
            {
                string dest = Path.Combine(Location, Path.GetFileName(file));
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
            return Setup();
        }

        protected virtual void ExecuteInitialCode()
        {
            string code = File.ReadAllText(InitializerScript);
            ECS.Instance.Execute(code);
        }

        private bool Setup()
        {
            try
            {
                if (string.IsNullOrEmpty(InitializerScript))
                {
                    throw new ArgumentNullException(nameof(InitializerScript));
                }

                if (!IsHidden(LibraryPath) && Directory.Exists(LibraryPath))
                {
                    foreach (var file in Directory.GetFiles(LibraryPath).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".dll"))
                    {
                        string dest = Path.Combine(Location, Path.GetFileName(file));
                        if (!File.Exists(dest))
                        {
                            File.Copy(file, dest);
                            LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
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
            finally
            {
                OnPropertyChanged("IsStarted");
            }
        }

        private static bool IsHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
    }
}
