using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Event_ECS_WPF.Misc
{
    public enum UpdateType
    {
        Manual,
        Automatic
    }

    public class Updater : NotifyPropertyChanged, IDisposable
    {
        private Updater()
        {
            Task.Run(() => UpdateThread());
        }

        private bool UpdateCall(ECSWrapper ecs)
        {
            try
            {
                return ecs.LoveUpdate();
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e.Message);
                return false;
            }
        }

        private void UpdateThread()
        {
            while (ECS.Instance.ProjectStarted)
            {
                while (UpdateType == UpdateType.Automatic && ECS.Instance.UseWrapper(UpdateCall, false)) ;
                Thread.Sleep(100);
            }
            this.Dispose();
        }

        public static bool StartUpdating()
        {
            if(Instance == null)
            {
                Instance = new Updater();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

        public static Updater Instance { get; private set; }

        public UpdateType UpdateType
        {
            get => m_updateType;
            set
            {
                m_updateType = value;
                OnPropertyChanged("UpdateType");
            }
        }
        private UpdateType m_updateType = UpdateType.Automatic;
    }
}
