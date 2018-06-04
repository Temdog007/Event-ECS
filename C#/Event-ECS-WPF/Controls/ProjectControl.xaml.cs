using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Projects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(ProjectControl));

        private IActionCommand m_addPathCommand;

        private ActionCommand<string> m_getPathCommand;

        private IActionCommand m_removePathCommand;

        public ProjectControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IActionCommand AddPathCommand => m_addPathCommand ?? (m_addPathCommand = new ActionCommand(AddPath));

        public IActionCommand GetPathCommand => m_getPathCommand ?? (m_getPathCommand = new ActionCommand<string>(GetPath));

        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public IActionCommand RemovePathCommand => m_removePathCommand ?? (m_removePathCommand = new ActionCommand<string>(RemovePath));

        private void AddPath()
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            if (!Project.ComponentPaths.Contains(dialog.SelectedPath))
                            {
                                Project.ComponentPaths.Add(dialog.SelectedPath);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog
            {
                Filter = "lua files (*.lua)|*.lua|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                Project.InitializerScript = dialog.FileName;
            }
        }

        private void GetPath(string propertyName)
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            Project.SetProperty(propertyName, dialog.SelectedPath);
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

        private void Project_MouseEnter(object sender, MouseEventArgs e)
        {
            RemovePathCommand.UpdateCanExecute(sender, e);
        }

        private void RemovePath(string path)
        {
            Project.ComponentPaths.Remove(path);
        }
    }
}
