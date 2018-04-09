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
        private static ECS m_instance;
        private object m_lock = new object();
        private ECS() { }

        public static ECS Instance => m_instance ?? (m_instance = new ECS());
        public bool ProjectStarted
        {
            get
            {
                lock (m_lock)
                {
                    return m_ecs != null;
                }
            }
        }

        public void Create(Project project)
        {
            lock (m_lock)
            {
                Dispose();

                m_ecs = new ECSWrapper();
                OnPropertyChanged("ProjectStarted");

                m_ecs.Initialize(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), project.Name, Convert.ToInt32(project.Type));
                LogManager.Instance.Add("Project Started");
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
                if (m_ecs != null)
                {
                    m_ecs.Dispose();
                    m_ecs = null;
                    OnPropertyChanged("ProjectStarted");
                    LogManager.Instance.Add("Project Stopped");
                }
            }
        }

        public T UseWrapper<T>(Func<ECSWrapper, T> action, bool throwException = true)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    throw new Exception("ECS Wrapper has been disposed");
                }
                else
                {
                    return action(m_ecs);
                }
            }
        }

        public void UseWrapper(Action<ECSWrapper> action)
        {
            lock(m_lock)
            {
                if (m_ecs == null)
                {
                    throw new Exception("ECS Wrapper has been disposed");
                }
                else
                {
                    action(m_ecs);
                }
            }
        }
    }
}
