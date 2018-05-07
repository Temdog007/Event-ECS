using System;
using System.Windows.Input;

namespace Event_ECS_WPF.Commands
{
    public interface IActionCommand : ICommand
    {
        void UpdateCanExecute(object sender, EventArgs e);
    }

    public interface IActionCommand<T> : IActionCommand
    {
        bool CanExecute(T parameter);
    }

    public class ActionCommand : IActionCommand
    {
        private readonly Action m_action;
        private readonly Func<bool> m_canExecute;

        public ActionCommand(Action action) : this(action, null) { }

        public ActionCommand(Action action, Func<bool> canExecute)
        {
            m_action = action;
            m_canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateCanExecute(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(sender, e);
        }
        
        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute();
        }

        public virtual void Execute(object parameter)
        {
            m_action?.Invoke();
        }
    }

    public class ActionCommand<T> : IActionCommand<T>
    {
        private readonly Action<T> m_action;
        private readonly Func<T, bool> m_canExecute;

        public ActionCommand(Action<T> action)
        {
            m_action = action;
            m_canExecute = obj => true;
        }

        public ActionCommand(Action<T> action, Func<T, bool> func)
        {
            m_action = action;
            m_canExecute = func;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateCanExecute(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(sender, e);
        }

        public bool CanExecute(T parameter)
        {
            return parameter == null ? true : m_canExecute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null && Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                return true;
            }
            else if(parameter is T)
            {
                return CanExecute((T)parameter);
            }
            return false;
        }

        public virtual void Execute(object parameter)
        {
            if (parameter is T)
            {
                m_action.Invoke((T)parameter);
            }
            else
            {
                UpdateCanExecute(this, EventArgs.Empty);
            }
        }
    }
}
