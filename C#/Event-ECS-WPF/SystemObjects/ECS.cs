using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;

namespace Event_ECS_WPF.SystemObjects
{
    public class ECS : NotifyPropertyChanged, IDisposable
    {
        public ECSWrapper m_ecs;

        private ECS() { }

        public static ECS Instance  => m_instance ?? (m_instance = new ECS());
        private static ECS m_instance;

        public void Create(Project project)
        {
            Dispose();

            m_ecs = new ECSWrapper();
            OnPropertyChanged("ProjectStarted");

            m_ecs.Initialize(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), project.Name, Convert.ToInt32(project.Type));
            LogManager.Instance.Add("Project Started");
        }

        public void Dispose()
        {
            if(m_ecs != null)
            {
                m_ecs.Dispose();
                m_ecs = null;
                OnPropertyChanged("ProjectStarted");
                LogManager.Instance.Add("Project Stopped");
            }
        }

        public bool ProjectStarted => m_ecs != null;

        public ECSWrapper Wrapper => m_ecs;
    }
}
