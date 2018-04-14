﻿using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Event_ECS_WPF.SystemObjects
{
    public class ECS : NotifyPropertyChanged, IDisposable
    {
        private static readonly TimeSpan WaitTimeSpan = TimeSpan.FromMilliseconds(10);

        private readonly ECSWrapper m_ecs;

        internal ECS(Project project)
        {
            m_ecs = new ECSWrapper(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                                    project.Name, Convert.ToInt32(project.Type), UpdateOnMainThread);
            LogManager.Instance.Add("Project Started");
            Instance = this;
        }

        private bool UpdateAction()
        {
            try
            {
                return m_ecs == null ? false : m_ecs.LoveUpdate();
            }
            catch(Exception)
            {
                return false;
            }
        }

        private bool UpdateOnMainThread()
        {
            try
            {
                DispatcherOperation operation = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Func<bool>(UpdateAction));
                while (operation.Wait(WaitTimeSpan) != DispatcherOperationStatus.Completed && m_ecs.GetAutoUpdate()) ;
                return (bool)operation.Result;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static ECS CreateInstance(Project project)
        {
            if (Instance == null)
            {
                return (Instance = new ECS(project));
            }
            throw new Exception("Previous instance wasn't disposed");
        }

        public static ECS Instance { get; private set; }

        public void Dispose()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            if (m_ecs != null)
            {
                Application.Current.Dispatcher.Invoke(() => m_ecs.Dispose());
                LogManager.Instance.Add("Project Stopped");
            }
        }

        public bool Update()
        {
            UseWrapper(UpdateAction, out bool rval);
            LogManager.Instance.Add("Manual Update");
            return rval;
        }

        public void SetAutoUpdate(bool value)
        {
            m_ecs.SetAutoUpdate(value);
        }

        public bool UseWrapper<T>(Func<ECSWrapper, T> action, out T t)
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

        public void UseWrapper(Action<ECSWrapper> action)
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

        private static bool UpdateAction(ECSWrapper ecs)
        {
            return ecs.LoveUpdate();
        }
    }
}
