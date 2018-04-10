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
                    Application.Current.Dispatcher.Invoke(() => m_ecs.Dispose());
                    LogManager.Instance.Add("Project Stopped");
                }
            }
        }
        public bool Update()
        {
            return Update(false);
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
                    Application.Current.Dispatcher.Invoke(() => action(m_ecs));
                }
            }
        }

        public async Task<bool> UseWrapperAsync<T>(Func<ECSWrapper, T> action)
        {
            bool canRun;
            lock(m_lock)
            {
                canRun = m_ecs != null;
            }

            if (!canRun)
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
                        if (m_ecs != null)
                        {
                            action(m_ecs);
                        }
                    }
                }));
                return true;
            }
        }

        internal Task StartAutoThread(CancellationToken token)
        {
            return Task.Run(() => UpdateThread(token), token);
        }
        private static bool UpdateAction(ECSWrapper ecs)
        {
            return ecs.LoveUpdate();
        }

        private Task<bool> Update(CancellationToken token)
        {
            return Task.Run(() => UseWrapperAsync(UpdateAction), token);
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
        private async void UpdateThread(CancellationToken token)
        {
            bool canRun = true;
            while (canRun)
            {
                try
                {
                    if (!await Update(token))
                    {
                        break;
                    }
                    token.ThrowIfCancellationRequested();
                    lock (m_lock)
                    {
                        canRun = m_ecs != null;
                    }
                }
                catch (Exception e)
                {
                    canRun = false;
                    LogManager.Instance.Add(e.Message);
                }
            }
        }
    }
}
