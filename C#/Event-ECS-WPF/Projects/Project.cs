using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private ObservableCollection<ValueContainer<string>> _eventsToIgnore;
        private string _initializer;

        private string _name;

        private string m_libraryPath;

        public Project()
        {
            Name = "New Project";
            ComponentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LibraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public static event ProjectStateChangeEvent ProjectStateChange;

        public static string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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

        [XmlArray("EventsToIgnore")]
        [XmlArrayItem("Event")]
        public ObservableCollection<ValueContainer<string>> EventsToIgnore
        {
            get => _eventsToIgnore ?? (_eventsToIgnore = new ObservableCollection<ValueContainer<string>>());
            set
            {
                _eventsToIgnore = new ObservableCollection<ValueContainer<string>>(value);
                OnPropertyChanged();
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

        public bool IsStarted => ECS.Instance.ProjectStarted;
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
        
        public void CopyComponentsToOutputPath()
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
                }
            }
        }

        public virtual bool Start()
        {
            if (Setup())
            {
                DispatchProjectStateChange(ProjectStateChangeArgs.Started);
                return true;
            }
            return false;
        }

        public virtual void Stop()
        {
            ECS.Instance.Dispose();
            ProjectStateChange?.Invoke(this, ProjectStateChangeArgs.Stopped);
            OnPropertyChanged("IsStarted");
        }

        protected virtual void CreateInstance()
        {
            string code = File.ReadAllText(InitializerScript);
            ECS.Instance.CreateInstance(code);
        }

        protected void DispatchProjectStateChange(ProjectStateChangeArgs args)
        {
            ProjectStateChange?.Invoke(this, args);
        }

        protected bool Setup()
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

                CreateInstance();
                ECS.Instance.UseWrapper(SetEventsToIgnore);
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

        private static bool IsHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
        
        private void SetEventsToIgnore(ECSWrapper ecs)
        {
            ecs.SetEventsToIgnore(EventsToIgnore.Select(s => s.Value).ToArray());
        }
    }
}
