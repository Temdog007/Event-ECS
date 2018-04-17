using System;
using System.Windows.Input;

namespace Event_ECS_WPF.Commands
{
    public class ActionCommand : ICommand
    {
        private Action m_action;

        public ActionCommand(Action action)
        {
            m_action = action;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateCanExecute(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(sender, e);
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            m_action?.Invoke();
        }
    }

    public class ActionCommand<T> : ICommand
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
            if (parameter == null)
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
            m_action.Invoke((T)parameter);
        }
    }
}
