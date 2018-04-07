using System;
using System.Windows;

namespace Event_ECS_WPF
{
    public class AsyncActionCommand<T> : ActionCommand<T>
    {
        public AsyncActionCommand(Action action) : base(action)
        {
        }

        public AsyncActionCommand(Action<T> action) : base(action)
        {
        }

        public AsyncActionCommand(Action<T> action, Func<T, bool> func) : base(action, func)
        {
        }

        public override void Execute(object parameter)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => base.Execute(parameter)));
        }
    }
}
