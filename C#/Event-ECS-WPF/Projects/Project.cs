using Event_ECS_WPF.Extensions;
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

        private string _outputPath = Location;

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

        /// <summary>
        /// Directory containing all of the lua component files
        /// </summary>
        [XmlArray("ComponentDirectories")]
        [XmlArrayItem("ComponentDirectory")]
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
                foreach (var componentPath in ComponentPaths.Select(c => c.Value))
                {
                    if (!componentPath.IsHidden() && Directory.Exists(componentPath))
                    {
                        foreach (var file in Directory.GetFiles(componentPath).Where(f => !f.IsHidden() && Path.GetExtension(f) == ".lua"))
                        {
                            yield return file;
                        }
                    }
                }
            }
        }

        [XmlArray("LibraryDirectories")]
        [XmlArrayItem("LibraryDirectory")]
        public ObservableCollection<ValueContainer<string>> LibraryPaths
        {
            get => m_libraryPaths ?? (m_libraryPaths = new ObservableCollection<ValueContainer<string>>());
            set
            {
                m_libraryPaths = value;
                OnPropertyChanged("LibraryPaths");
            }
        }

        public IEnumerable<string> AllLibraryPaths
        {
            get
            {
                yield return Location;
                foreach(var path in LibraryPaths)
                {
                    yield return path;
                }
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
        public string OutputPath
        {
            get => _outputPath;
            set
            {
                _outputPath = value;
                OnPropertyChanged();
            }
        }

        public virtual ProjectType Type => ProjectType.NORMAL;
        
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

        public bool CheckOutDir()
        {
            if(!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            return true;
        }

        public virtual bool Start()
        {
            throw new NotImplementedException("Start is not applicable for a generic project");
        }
    }
}
