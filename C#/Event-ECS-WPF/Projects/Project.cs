using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
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
            "colorComponent",
            "finalizerComponent"
        };

        private string _componentPath;

        private string _name;

        private string m_libraryPath;

        public Project()
        {
            Name = "New Project";
            ComponentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LibraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Systems.Add("Default");
        }

        public static event ProjectStateChangeEvent ProjectStateChange;

        /// <summary>
        /// Directory containing all of the lua component files
        /// </summary>
        [XmlElement]
        public string ComponentPath
        {
            get => _componentPath;
            set
            {
                _componentPath = value;
                OnPropertyChanged("ComponentPath");
                OnPropertyChanged("Components");
            }
        }

        public IEnumerable<string> Files
        {
            get
            {
                if (!IsHidden(ComponentPath) && Directory.Exists(ComponentPath))
                {
                    foreach (var file in Directory.GetFiles(ComponentPath).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".lua"))
                    {
                        yield return file;
                    }
                }
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

        [XmlElement]
        public string InitializerComponent
        {
            get => _initializer;
            set
            {
                _initializer = value;
                OnPropertyChanged("IntializerComponent");
            }
        }private string _initializer;

        [XmlArray("Systems")]
        [XmlArrayItem("System")]
        public ObservableCollection<StringWrapper> Systems
        {
            get => m_systems;
            set
            {
                m_systems = value;
                OnPropertyChanged("Systems");
            }
        } private ObservableCollection<StringWrapper> m_systems = new ObservableCollection<StringWrapper>();

        public virtual ProjectType Type
        {
            get => ProjectType.NORMAL;
        }

        public bool IsStarted => ECS.Instance.ProjectStarted;

        protected void DispatchProjectStateChange(ProjectStateChangeArgs args)
        {
            ProjectStateChange?.Invoke(this, args);
        }

        protected void CreateSystems(ECSWrapper ecs)
        {
            ecs.AddSystems(Systems.Select(s => (string)s).ToArray());
        }

        public virtual bool Start()
        {
            if(Setup())
            {
                ECS.Instance.UseWrapper(CreateSystems);
                DispatchProjectStateChange(ProjectStateChangeArgs.Started);
                return true;
            }
            return false;
        }

        protected bool Setup()
        {
            try
            {
                string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!IsHidden(LibraryPath) && Directory.Exists(LibraryPath))
                {
                    foreach (var file in Directory.GetFiles(LibraryPath).Where(f => !IsHidden(f) && Path.GetExtension(f) == ".dll"))
                    {
                        string dest = Path.Combine(location, Path.GetFileName(file));
                        if (!File.Exists(dest))
                        {
                            File.Copy(file, dest);
                            LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                        }
                    }
                }

                foreach(string file in Files)
                {
                    string dest = Path.Combine(location, Path.GetFileName(file));
                    if (!File.Exists(dest) || File.GetLastWriteTimeUtc(dest) != (File.GetLastWriteTimeUtc(file)))
                    {
                        File.Copy(file, dest, true);
                        var now = DateTime.Now;
                        File.SetLastWriteTimeUtc(file, now);
                        File.SetLastWriteTimeUtc(dest, now);
                        LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                    }
                }

                ECS.Instance.CreateInstance();
                return true;
            }
            catch (Exception e)
            {
                ECS.Instance.Dispose();
                LogManager.Instance.Add(LogLevel.High, e.Message);
                return false;
            }
            finally
            {
                OnPropertyChanged("IsStarted");
            }
        }

        public virtual void Stop()
        {
            ECS.Instance.Dispose();
            ProjectStateChange?.Invoke(this, ProjectStateChangeArgs.Stopped);
            OnPropertyChanged("IsStarted");
        }

        private static bool IsHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
    }

    public class StringWrapper
    {
        public string Value { get; set; }

        public static implicit operator string(StringWrapper w)
        {
            return w.Value;
        }

        public static implicit operator StringWrapper(string str)
        {
            return new StringWrapper() { Value = str };
        }
    }
}
