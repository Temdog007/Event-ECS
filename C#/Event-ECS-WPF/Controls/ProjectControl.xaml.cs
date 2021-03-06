﻿using Event_ECS_WPF.Projects;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectControl));

        public ProjectControl()
        {
            InitializeComponent();
        }

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private void AddComponentDirectory(object sender, RoutedEventArgs e)
        {
            using (Forms.FolderBrowserDialog dialog = new Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = Project.OutputPath;
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        Project.OutputPath = dialog.SelectedPath;
                        break;
                }
            }
        }

        private void ClearOutputDirectory(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(Project.OutputPath))
            {
                Directory.Delete(Project.OutputPath, true);
            }
        }
    }
}
