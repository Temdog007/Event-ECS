using Event_ECS_WPF.Commands;
using System.Windows.Input;

namespace Event_ECS_WPF.SystemObjects
{
    public partial class ECS : NotifyPropertyChanged
    {
        private static ECS s_instance;

        private ActionCommand m_resetProjectCommand;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public ICommand ResetProjectCommand => m_resetProjectCommand ?? (m_resetProjectCommand = new ActionCommand(Reset));

        private bool m_bIsConnected;

        public bool IsConnected
        {
            get => m_bIsConnected;
            set
            {
                m_bIsConnected = value;
                OnPropertyChanged();
            }
        }
    }
}
