using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
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
        public EntityComponentSystemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(EntityComponentSystemControl));

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public IActionCommand AddEntityCommand => m_addEntityCommand ?? (m_addEntityCommand = new ActionCommand(AddEntity));
        private IActionCommand m_addEntityCommand;

        public IActionCommand RemoveEntityCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand(RemoveEntity, CanRemoveEntity));
        private IActionCommand m_removeEntityCommand;

        public bool CanRemoveEntity()
        {
            return SelectedEntity != null;
        }

        public Entity SelectedEntity
        {
            get => m_selectedEntity; set
            {
                m_selectedEntity = value;
                OnPropertyChanged("SelectedEntity");
            }
        } private Entity m_selectedEntity;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string AddEntityFunc(ECSWrapper ecs)
        {
            return ecs.AddEntity();
        }

        private bool RemoveEntityFunc(ECSWrapper ecs, int entityID)
        {
            try
            {
                return ecs.RemoveEntity(entityID);
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, "Cannot remove entity: {0}", e.Message);
                return false;
            }
        }

        private void AddEntity()
        {
            if (ECS.Instance.UseWrapper(AddEntityFunc, out string str))
            {
                LogManager.Instance.Add("Added entity: {0}", str);
                EntityComponentSystem.Deserialize();
            }
        }

        private void RemoveEntity()
        {
            if(ECS.Instance.UseWrapper(RemoveEntityFunc, SelectedEntity.ID, out bool removed))
            {
                LogManager.Instance.Add("Entity removed: {0}", removed);
                EntityComponentSystem.Deserialize();
            }
        }

        private void System_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEntityCommand.UpdateCanExecute(this, e);
            RemoveEntityCommand.UpdateCanExecute(this, e);
        }
    }
}
