using Event_ECS_WPF.Projects;
using System;

namespace Event_ECS_WPF.SystemObjects
{
    public class ECSUpdater : NotifyPropertyChanged, IDisposable
    {
        private static ECSUpdater m_instance;
        private static object m_lock = new object();
        private bool m_disposed = false;
        private ECS m_ecs;

        private ECSUpdater(Project project)
        {
            m_ecs = new ECS(project ?? throw new ArgumentException(nameof(Project)));
            Instance = this;
        }
        public bool Disposed { get => m_disposed; private set => m_disposed = value; }

        private static ECSUpdater Instance { get; set; }

        public static ECSUpdater CreateInstance(Project project = null)
        {
            lock (m_lock)
            {
                m_instance?.Dispose();
                return (m_instance = new ECSUpdater(project));
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
                }
            }
        }
    }
}
