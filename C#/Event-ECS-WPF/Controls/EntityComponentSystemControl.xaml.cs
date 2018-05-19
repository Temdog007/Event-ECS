using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for SystemControl.xaml
    /// </summary>
    public partial class EntityComponentSystemControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(EntityComponentSystemControl));

        public static readonly DependencyProperty ProjectProperty =
           DependencyProperty.Register("Project", typeof(Project), typeof(EntityComponentSystemControl));

        private IActionCommand m_addEntityCommand;

        private IActionCommand m_removeEntityCommand;

        private IActionCommand m_serializeCommand;

        private Entity m_selectedEntity;

        public EntityComponentSystemControl()
        {
            InitializeComponent();

            ECS.DeserializeRequested += ECS_DeserializeRequested;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand(AddEntity));

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public IActionCommand RemoveEntityCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand(RemoveEntity, CanRemoveEntity));

        public Entity SelectedEntity
        {
            get => m_selectedEntity; set
            {
                m_selectedEntity = value;
                OnPropertyChanged("SelectedEntity");
            }
        }

        public IActionCommand SerializeCommand => m_serializeCommand ?? (m_serializeCommand = new ActionCommand(Serialize));

        public bool CanRemoveEntity()
        {
            return SelectedEntity != null;
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddEntity()
        {
            if (ECS.Instance.UseWrapper(AddEntityFunc, out string str))
            {
                LogManager.Instance.Add("Added entity: {0}", str);
                EntityComponentSystem.Deserialize();
            }
        }

        private string AddEntityFunc(ECSWrapper ecs)
        {
            return ecs.AddEntity(EntityComponentSystem.Name);
        }

        private void ECS_DeserializeRequested()
        {
            Dispatcher.BeginInvoke(new Action(() => EntityComponentSystem.Deserialize()));
        }

        private void RemoveEntity()
        {
            if (ECS.Instance.UseWrapper(RemoveEntityFunc, SelectedEntity.ID, out bool removed))
            {
                LogManager.Instance.Add("Entity removed: {0}", removed);
                EntityComponentSystem.Deserialize();
            }
        }

        private bool RemoveEntityFunc(ECSWrapper ecs, int entityID)
        {
            try
            {
                return ecs.RemoveEntity(EntityComponentSystem.Name, entityID);
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, "Cannot remove entity: {0}", e.Message);
                return false;
            }
        }

        private void Serialize()
        {
            if (ECS.Instance.UseWrapper(ecs => ecs.SerializeSystem(EntityComponentSystem.Name), out string data))
            {
                EntityComponentSystem.Deserialize(data.Split('\n'));
                LogManager.Instance.Add(data, LogLevel.Low);
            }
        }

        private void System_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEntityCommand.UpdateCanExecute(this, e);
            RemoveEntityCommand.UpdateCanExecute(this, e);
        }
    }
}
