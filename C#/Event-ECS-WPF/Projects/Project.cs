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
                OnPropertyChanged("Components");
            }
        }

        public IEnumerable<string> Files
        {
            get
            {
                if (!isHidden(ComponentPath) && Directory.Exists(ComponentPath))
                {
                    foreach (var file in Directory.GetFiles(ComponentPath).Where(f => !isHidden(f) && Path.GetExtension(f) == ".lua"))
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
                foreach(var file in Files)
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

        public bool IsStarted => ECS.Instance.ProjectStarted;

        protected void DispatchProjectStateChange(ProjectStateChangeArgs args)
        {
            ProjectStateChange?.Invoke(this, args);
        }

        public virtual bool Start()
        {
            if(Setup())
            {
                ECS.Instance.UseWrapper(ecs =>
                {
                    foreach (var component in Components)
                    {
                        try
                        {
                            ecs.RegisterComponent(component, false);
                        }
                        catch (Exception e)
                        {
                            LogManager.Instance.Add(string.Format("Failed to register component {0}\n{1}", component, e.Message));
                        }
                    }
                    InitializeECS(ecs);
                });
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

        public void InitializeECS(ECSWrapper ecs)
        {
            if (!string.IsNullOrWhiteSpace(IntializerComponent))
            {
                string[] enData = ecs.AddEntity().Split(EntityComponentSystem.Delim);
                int entityID = int.Parse(enData[0]);
                ecs.SetEntityString(entityID, "name", "MainEntity");
                ecs.AddComponent(entityID, IntializerComponent);
                ecs.Serialize();
            }
        }

        private static bool isHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }
    }
}
