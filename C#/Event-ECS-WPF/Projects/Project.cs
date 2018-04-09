using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{
    public enum UpdateType
    {
        MANUAL,
        AUTOMATIC
    }

    [XmlRoot("Project")]
    public class Project : NotifyPropertyChanged
    {
        private string _componentPath;
        private string _name;
        private UpdateType _updateType;
        private ObservableCollection<string> m_extensions = new ObservableCollection<string>();

        public Project()
        {
            Name = "New Project";
            ComponentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            UpdateType = UpdateType.MANUAL;
        }

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

        [XmlElement]
        public UpdateType UpdateType
        {
            get => _updateType;
            set
            {
                _updateType = value;
                OnPropertyChanged("UpdateType");
            }
        }

        public virtual void Start()
        {
            if (Directory.Exists(ComponentPath))
            {
                foreach (var file in Directory.GetFiles(ComponentPath).Where(f => Path.GetExtension(f) == ".lua"))
                {
                    File.Copy(file, Assembly.GetExecutingAssembly().Location);
                }
            }
        }
    }
}
