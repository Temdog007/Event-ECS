﻿using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Projects;
using Event_ECS_WPF.SystemObjects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectMenuControl.xaml
    /// </summary>
    public partial class ProjectMenuControl : UserControl, INotifyPropertyChanged
    {

        public static readonly DependencyProperty ProjectProperty =
                            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectMenuControl));

        private ICommand m_projectCommand;

        public ProjectMenuControl()
        {
            InitializeComponent();
            ECS.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public ICommand ProjectActionCommand => m_projectCommand ?? (m_projectCommand = new ActionCommand(ProjectAction));

        public string ProjectText => ECS.Instance.IsApplicationRunning ? " Stop" : "Start";

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ProjectText");
        }

        private void ProjectAction()
        {
            if (ECS.Instance.IsApplicationRunning)
            {
                Project.Stop();
            }
            else
            {
                Project.Start();
            }
        }
    }
}
