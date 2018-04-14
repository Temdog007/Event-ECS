using Event_ECS_WPF.Logger;
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
        private string _componentPath;

        private string _name;

        private ObservableCollection<string> m_extensions = new ObservableCollection<string>();

        private string m_libraryPath;

        public Project()
        {
            Name = "New Project";
            ComponentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LibraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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

        public virtual bool Start()
        {
            try
            {
                string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                List<string> componentsToRegister = new List<string>();
                if (!isHidden(ComponentPath) && Directory.Exists(ComponentPath))
                {
                    foreach (var file in Directory.GetFiles(ComponentPath).Where(f => !isHidden(f) && Path.GetExtension(f) == ".lua"))
                    {
                        string component = Path.GetFileName(file);
                        string dest = Path.Combine(location, component);
                        if (!File.Exists(dest))
                        {
                            File.Copy(file, dest);
                            LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                        }
                        componentsToRegister.Add(Path.GetFileNameWithoutExtension(component));
                    }
                }

                if (!isHidden(LibraryPath) && Directory.Exists(LibraryPath))
                {
                    foreach (var file in Directory.GetFiles(LibraryPath).Where(f => !isHidden(f) && Path.GetExtension(f) == ".dll"))
                    {
                        string dest = Path.Combine(location, Path.GetFileName(file));
                        if (!File.Exists(dest))
                        {
                            File.Copy(file, dest);
                            LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                        }
                    }
                }

                ECS.CreateInstance(this);
                ECS.Instance.UseWrapper(ecs => 
                {
                    foreach(var file in componentsToRegister)
                    {
                        ecs.RegisterComponent(file, false);
                    }
                });
                ProjectStateChange?.Invoke(this, ProjectStateChangeArgs.Started);
                return true;
            }
            catch (Exception e)
            {
                ECS.Instance?.Dispose();
                LogManager.Instance.Add(LogLevel.High, e.Message);
                return false;
            }
        }

        public virtual void Stop()
        {
            ECS.Instance?.Dispose();
            ProjectStateChange?.Invoke(this, ProjectStateChangeArgs.Stopped);
        }

        private static bool isHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
    }
}
