﻿using Event_ECS_WPF.Logger;
using System;
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
        private string _componentPath;
        private string _name;
        
        private ObservableCollection<string> m_extensions = new ObservableCollection<string>();

        public Project()
        {
            Name = "New Project";
            ComponentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LibraryPath = string.Empty;
            
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

        [XmlElement]
        public string LibraryPath
        {
            get => m_libraryPath;
            set
            {
                m_libraryPath = value;
                OnPropertyChanged("LibraryPath");
            }
        } private string m_libraryPath;

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

        private static bool isHidden(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }

        public virtual void Start()
        {
            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!isHidden(ComponentPath) && Directory.Exists(ComponentPath))
            {
                foreach (var file in Directory.GetFiles(ComponentPath).Where(f => !isHidden(f) && Path.GetExtension(f) == ".lua"))
                {
                    string dest = Path.Combine(location, Path.GetFileName(file));
                    if (!File.Exists(dest))
                    {
                        File.Copy(file, dest);
                        LogManager.Instance.Add("Copied {0} to {1}", file, dest);
                    }
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
                        LogManager.Instance.Add("Copied {0} to {1}", file, dest);
                    }
                }
            }
        }
    }
}