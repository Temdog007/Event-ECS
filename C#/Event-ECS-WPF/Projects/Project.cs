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

        [XmlElement]
        public string IntializerComponent
        {
            get => _initializer;
            set
            {
                _initializer = value;
                OnPropertyChanged("IntializerComponent");
            }
        }private string _initializer;

        public virtual ProjectType Type
        {
            get => ProjectType.NORMAL;
        }

        public bool IsStarted => ECS.Instance != null;

        protected void DispatchProjectStateChange(ProjectStateChangeArgs args)
        {
            ProjectStateChange?.Invoke(this, args);
        }

        public virtual bool Start()
        {
            if(Start(out IEnumerable<string> componentsToRegsiter))
            {
                ECS.Instance.UseWrapper(ecs =>
                {
                    foreach (var file in componentsToRegsiter)
                    {
                        ecs.RegisterComponent(file, false);
                    }
                    InitializeECS(ecs);
                });
                DispatchProjectStateChange(ProjectStateChangeArgs.Started);
                return true;
            }
            return false;
        }

        protected bool Start(out IEnumerable<string> pComponentsToRegister)
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
                        if (!File.Exists(dest) || File.GetCreationTime(dest) != (File.GetCreationTime(file)))
                        {
                            File.Copy(file, dest, true);
                            var now = DateTime.Now;
                            File.SetCreationTime(file, now);
                            File.SetCreationTime(dest, now);
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
                pComponentsToRegister = componentsToRegister;
                return true;
            }
            catch (Exception e)
            {
                ECS.Instance?.Dispose();
                LogManager.Instance.Add(LogLevel.High, e.Message);
                pComponentsToRegister = Enumerable.Empty<string>();
                return false;
            }
            finally
            {
                OnPropertyChanged("IsStarted");
            }
        }

        public virtual void Stop()
        {
            ECS.Instance?.Dispose();
            ProjectStateChange?.Invoke(this, ProjectStateChangeArgs.Stopped);
            OnPropertyChanged("IsStarted");
        }

        public void InitializeECS(ECSWrapper ecs)
        {
            if (!string.IsNullOrWhiteSpace(IntializerComponent))
            {
                string[] enData = ecs.AddEntity().Split(EntityComponentSystem.Delim);
                int entityID = int.Parse(enData[0]);
                ecs.SetEntityString(entityID, "name", "MainEntity");
                ecs.AddComponent(entityID, IntializerComponent);
            }
        }

        private static bool isHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
    }
}
