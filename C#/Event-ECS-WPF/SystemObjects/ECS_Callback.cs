using Event_ECS_Lib;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Event_ECS_WPF.SystemObjects
{
    public abstract class ECS_Callback : NotifyPropertyChanged, IECSWrapperCallback
    {
        internal ECS_Callback() { }

        delegate void InvokeDelegate(string funcName, object response);

        public List<Func<string, object, bool>> Listeners { get; } = new List<Func<string, object, bool>>();

        public void AddEntity(string value)
        {
            Invoke("AddEntity", value);
        }

        public void BroadcastEvent(string value)
        {
            Invoke("BroadcastEvent", value);
        }

        public void DispatchEvent(int value)
        {
            Invoke("DispatchEvent", value);
        }

        public abstract void Dispose();

        public void GetClassName(string value)
        {
            Invoke("GetClassName", value);
        }

        public void GetComponentBool(bool value)
        {
            Invoke("GetComponentBool", value);
        }

        public void GetComponentNumber(double value)
        {
            Invoke("GetComponentNumber", value);
        }

        public void GetComponentString(string value)
        {
            Invoke("GetComponentString", value);
        }

        public void GetEntityBool(bool value)
        {
            Invoke("GetEntityBool", value);
        }

        public void GetEntityNumber(double value)
        {
            Invoke("GetEntityNumber", value);
        }

        public void GetEntityString(string value)
        {
            Invoke("GetEntityString", value);
        }

        public void GetSystemBool(bool value)
        {
            Invoke("GetSystemBool", value);
        }

        public void GetSystemNumber(double value)
        {
            Invoke("GetSystemNumber", value);
        }

        public void GetSystemString(string value)
        {
            Invoke("GetSystemString", value);
        }

        public void IsComponentEnabled(bool value)
        {
            Invoke("IsComponentEnabled", value);
        }

        public void IsDisposing(bool value)
        {
            Invoke("IsDisposing", value);
        }

        public void IsEntityEnabled(bool value)
        {
            Invoke("IsEntityEnabled", value);
        }

        public void IsLoggingEvents(bool value)
        {
            Invoke("IsLoggingEvents", value);
        }

        public void IsStarted(bool value)
        {
            Invoke("IsStarted", value);
        }

        public void IsUpdatingAutomatically(bool value)
        {
            Invoke("IsUpdatingAutomatically", value);
        }

        public void IsSystemEnabled(bool value)
        {
            Invoke("IsSystemEnabled", value);
        }

        public abstract void LogEvent(string log);

        public void RemoveComponent(bool value)
        {
            Invoke("RemoveComponent", value);
        }

        public void RemoveEntity(bool value)
        {
            Invoke("RemoveEntity", value);
        }

        public void Serialize(string[] value)
        {
            Invoke("Serialize", value);
        }

        public void SerializeComponent(string value)
        {
            Invoke("SerializeComponent", value);
        }

        public void SerializeEntity(string value)
        {
            Invoke("SerializeEntity", value);
        }

        public void SerializeSystem(string value)
        {
            Invoke("SerializeSystem", value);
        }

        public void Update(int value)
        {
            Invoke("Update", value);
        }

        private void DoInvoke(string funcName, object response)
        {
            foreach(var listener in Listeners.ToArray())
            {
                if(listener(funcName, response))
                {
                    Listeners.Remove(listener);
                }
            }
        }

        private void Invoke(string funcName, object response)
        {
            InvokeDelegate d = DoInvoke;
            Application.Current.Dispatcher.BeginInvoke(d, funcName, response);
        }
    }
}
