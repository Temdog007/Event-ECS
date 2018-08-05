using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for AddComponentControl.xaml
    /// </summary>
    public partial class AddComponentControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AddComponentControl));

        public static readonly DependencyProperty ComponentsProperty =
            DependencyProperty.Register("Components", typeof(IEnumerable<string>), typeof(AddComponentControl));

        public static readonly DependencyProperty RelevantLettersProperty =
                    DependencyProperty.Register("RelevantLetters", typeof(IEnumerable<char>), typeof(AddComponentControl));

        private char m_selectedLetter = default(char);

        public AddComponentControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IEnumerable<string> Components
        {
            get { return (IEnumerable<string>)GetValue(ComponentsProperty); }
            set { SetValue(ComponentsProperty, value); }
        }

        public IEnumerable<char> RelevantLetters
        {
            get { return (IEnumerable<char>)GetValue(RelevantLettersProperty); }
            set { SetValue(RelevantLettersProperty, value); }
        }

        public IEnumerable<string> SelectableComponents
        {
            get
            {
                return Components?.Where(comp => comp.StartsWith(SelectedLetter.ToString(), StringComparison.OrdinalIgnoreCase)) ?? Enumerable.Empty<string>();
            }
        }

        public char SelectedLetter
        {
            get => m_selectedLetter;
            set
            {
                m_selectedLetter = value;
                OnPropertyChanged("SelectedLetter");
                OnPropertyChanged("SelectableComponents");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var selectedItem = button.Content;
                if (Command?.CanExecute(selectedItem) ?? false)
                {
                    Command.Execute(selectedItem);
                }
            }
        }
    }
}
