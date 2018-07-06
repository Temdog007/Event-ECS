using Event_ECS_WPF.SystemObjects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ComponentVariableStringControl.xaml
    /// </summary>
    public partial class ComponentVariableStringControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.Register("ComponentVariable", 
                typeof(IComponentVariable), typeof(ComponentVariableStringControl),
                new PropertyMetadata(null, OnVariableChanged));

        private string m_text = string.Empty;

        public ComponentVariableStringControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_canUpdate = false;
        public bool CanUpdate
        {
            get => m_canUpdate;
            set
            {
                if(m_canUpdate != value)
                {
                    return;
                }

                m_canUpdate = value;
                if(CanUpdate && ComponentVariable != null && (string)ComponentVariable.Value != Text)
                {
                    ComponentVariable.Value = Text;
                }
            }
        }

        public IComponentVariable ComponentVariable
        {
            get { return (IComponentVariable)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                if (CanUpdate && ComponentVariable != null)
                {
                    ComponentVariable.Value = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        private static void OnVariableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ComponentVariableStringControl ctrl && string.IsNullOrWhiteSpace(ctrl.Text))
            {
                ctrl.CanUpdate = false;
                ctrl.Text = ctrl.ComponentVariable.Value.ToString();
                ctrl.CanUpdate = true;
            }
        }

        private void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            CanUpdate = false;
        }

        private void Control_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            CanUpdate = true;
        }
    }
}
