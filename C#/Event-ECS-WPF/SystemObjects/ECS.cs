using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Event_ECS_WPF.SystemObjects
{
    public class ECS : NotifyPropertyChanged, IDisposable
    {
        private static readonly object m_lock = new object();
        private readonly ECSWrapper m_ecs;
        internal ECS(Project project)
        {
            m_ecs = new ECSWrapper();

            m_ecs.Initialize(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), project.Name, Convert.ToInt32(project.Type));
            LogManager.Instance.Add("Project Started");
            Instance = this;
        }

        public static ECS Instance { get; private set; }

        public void Dispose()
        {
            lock (m_lock)
            {
                if(Instance == this)
                {
                    Instance = null;
                }

                if (m_ecs != null)
                {
                    m_ecs.Dispose();
                    LogManager.Instance.Add("Project Stopped");
                }
            }
        }
        internal void StartAutoThread(CancellationToken token)
        {
            Task.Run(() => UpdateThread(token), token);
        }

        public bool Update()
        {
            return Update(false);
        }

        private async Task<bool> Update(CancellationToken token)
        {
            return await Task.Run(() => UseWrapperAsync(UpdateAction));
        }

        private bool Update(bool automatic)
        {
            UseWrapper(UpdateAction, out bool rval);
            if (!automatic)
            {
                LogManager.Instance.Add("Manual Update");
            }
            return rval;
        }
        
        public bool UseWrapper<T>(Func<ECSWrapper, T> action, out T t)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add("Project has not been started. Cannot run function");
                    t = default(T);
                    return false;
                }
                else
                {
                    t = Application.Current.Dispatcher.Invoke(() => action(m_ecs));
                    return true;
                }
            }
        }

        public async Task<bool> UseWrapperAsync<T>(Func<ECSWrapper, T> action)
        {

            if (m_ecs == null)
            {
                LogManager.Instance.Add("Project has not been started. Cannot run function");
                return false;
            }
            else
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    lock (m_lock)
                    {
                        action(m_ecs);
                    }
                }));
                return true;
            }
        }

        public void UseWrapper(Action<ECSWrapper> action)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add("Project has not been started. Cannot run function");
                }
                else
                {
                    action(m_ecs);
                }
            }
        }

        private static bool UpdateAction(ECSWrapper ecs)
        {
            return ecs.LoveUpdate();
        }
        private async Task UpdateThread(CancellationToken token)
        {
            while (m_ecs != null)
            {
                if(!await Update(token))
                {
                    break;
                }
                token.ThrowIfCancellationRequested();
            }
        }
    }
}
