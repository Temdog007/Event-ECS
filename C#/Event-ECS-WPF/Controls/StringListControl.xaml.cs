using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Misc;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for StringListControl.xaml
    /// </summary>
    public partial class StringListControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(ObservableCollection<ValueContainer<string>>), typeof(StringListControl));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(StringListControl));

        private IActionCommand m_addPathCommand;

        private IActionCommand m_removePathCommand;

        public StringListControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddPathCommand => m_addPathCommand ?? (m_addPathCommand = new ActionCommand(AddPath));

        public ObservableCollection<ValueContainer<string>> List
        {
            get { return (ObservableCollection<ValueContainer<string>>)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public IActionCommand RemovePathCommand => m_removePathCommand ?? (m_removePathCommand = new ActionCommand<string>(RemovePath));

        private void AddPath()
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = List.LastOrDefault();
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            if (!List.Contains(dialog.SelectedPath))
                            {
                                List.Add(dialog.SelectedPath);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void List_MouseEnter(object sender, MouseEventArgs e)
        {
            RemovePathCommand.UpdateCanExecute(sender, e);
        }

        private void RemovePath(string path)
        {
            List.Remove(path);
        }
    }
}
