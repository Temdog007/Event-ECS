using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using Event_ECS_WPF.SystemObjects.Communication;
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
            "finalizerComponent"
        };

        protected ObservableCollection<char> componentLetters;

        private ObservableCollection<ValueContainer<string>> _componentPath;

        private string _name;

        private string _outputPath = Location;

        private bool includeDirectories;

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

        public IEnumerable<string> AllLibraryPaths
        {
            get
            {
                yield return Location;
                foreach (var path in LibraryPaths)
                {
                    yield return path;
                }
            }
        }

        public ObservableCollection<char> ComponentLetters => componentLetters ?? (componentLetters = new ObservableCollection<char>());

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
                    foreach (var file in GetFilesInDirectory(componentPath))
                    {
                        yield return file;
                    }
                }
            }
        }

        [XmlAttribute]
        public bool IncludeDirectories
        {
            get => includeDirectories;
            set
            {
                includeDirectories = value;
                OnPropertyChanged("IncludeDirectories");
            }
        }

        [XmlArray("LibraryDirectories")]
        [XmlArrayItem("LibraryDirectory")]
        public ObservableCollection<ValueContainer<string>> LibraryPaths
        {
            get => m_libraryPaths ?? (m_libraryPaths = new System.Collections.ObjectModel.ObservableCollection<ValueContainer<string>>());
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

        public static bool IsCopyable(string file)
        {
            switch (Path.GetExtension(file))
            {
                case ".lua":
                case ".png":
                case ".ogg":
                    return true;
                default:
                    break;
            }
            return false;
        }

        public bool CheckOutDir()
        {
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            return true;
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
            throw new NotImplementedException("Start is not applicable for a generic project");
        }

        private static IEnumerable<string> GetComponents(string directory)
        {
            foreach (var file in Directory.GetFiles(directory).Where(f => !f.IsHidden() && IsCopyable(f)))
            {
                yield return file;
            }
        }

        private IEnumerable<string> GetFilesInDirectory(string componentDir)
        {
            if (!componentDir.IsHidden() && Directory.Exists(componentDir))
            {
                foreach (var file in GetComponents(componentDir))
                {
                    yield return file;
                }

                if (IncludeDirectories)
                {
                    foreach (var dir in Directory.GetDirectories(componentDir))
                    {
                        foreach (var file in GetFilesInDirectory(dir))
                        {
                            yield return file;
                        }
                    }
                }
            }
        }
    }
}
