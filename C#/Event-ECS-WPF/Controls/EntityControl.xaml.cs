using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects;
using EventECSWrapper;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for Entity.xaml
    /// </summary>
    public partial class EntityControl : UserControl
    {
        public EntityControl()
        {
            InitializeComponent();
        }

        public Entity Entity
        {
            get { return (Entity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(EntityControl));

        public ICommand RemoveEntityCommand => m_removeEntityCommand ?? (m_removeEntityCommand = new ActionCommand(RemoveEntity));
        private ICommand m_removeEntityCommand;

        private bool RemoveEntityFunc(ECSWrapper ecs)
        {
            return ecs.RemoveEntity(Entity.ID);
        }

        private void RemoveEntity()
        {
            try
            {
                ECS.Instance.UseWrapper(RemoveEntityFunc, out bool removed);
                Entity.System.Deserialize();
                LogManager.Instance.Add("Entities removed: {0}", removed);
            }
            catch(Exception e)
            {
                LogManager.Instance.Add("Can't remove entity: {0}\n{1}", Entity.Name, e.Message);
            }
        }

        public ICommand AddComponentCommand => m_addComponentCommand ?? (m_addComponentCommand = new ActionCommand<string>(AddComponent));
        private ICommand m_addComponentCommand;

        private void AddComponent(string param)
        {
            ECS.Instance.UseWrapper(ecs => ecs.AddComponent(Entity.ID, param));
            Entity.System.Deserialize();
        }
    }
}
