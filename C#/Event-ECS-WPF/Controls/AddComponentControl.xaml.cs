using Event_ECS_WPF.Projects;
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
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(AddComponentControl), new PropertyMetadata(null, OnProjectChanged));

        private static void OnProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is AddComponentControl ctrl)
            {
                ctrl.OnPropertyChanged("RelevantLetters");
                ctrl.SelectedLetter = ctrl.RelevantLetters.FirstOrDefault();
                ctrl.OnPropertyChanged("SelectableComponents");
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AddComponentControl));

        private char m_selectedLetter = default(char);

        public AddComponentControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IEnumerable<char> RelevantLetters
        {
            get
            {
                if (Project == null)
                {
                    yield break;
                }
                for (char c = 'A'; c <= 'Z'; ++c)
                {
                    if (Project.Components.Any(comp => comp.StartsWith(c.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        yield return c;
                    }
                }
            }
        }

        public IEnumerable<string> SelectableComponents
        {
            get
            {
                return Project?.Components.Where(comp => comp.StartsWith(SelectedLetter.ToString(), StringComparison.OrdinalIgnoreCase));
            }
        }

        public char SelectedLetter
        {
            get => m_selectedLetter;
            set
            {
                if (Project == null)
                {
                    return;
                }
                m_selectedLetter = value;
                OnPropertyChanged("SelectedLetter");
                OnPropertyChanged("SelectableComponents");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
            {
                var selectedItem = items[0];
                if (Command?.CanExecute(selectedItem) ?? false)
                {
                    Command.Execute(selectedItem);
                }
            }
        }
    }
}
