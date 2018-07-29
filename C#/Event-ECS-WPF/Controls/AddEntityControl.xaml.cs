using Event_ECS_WPF.SystemObjects;
using Event_ECS_WPF.SystemObjects.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for AddEntityControl.xaml
    /// </summary>
    public partial class AddEntityControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("EntityComponentSystem", typeof(EntityComponentSystem), typeof(AddEntityControl));

        public AddEntityControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EntityComponentSystem EntityComponentSystem
        {
            get { return (EntityComponentSystem)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                ECS.Instance.AddEntity(EntityComponentSystem.ID, button.Content.ToString());
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}