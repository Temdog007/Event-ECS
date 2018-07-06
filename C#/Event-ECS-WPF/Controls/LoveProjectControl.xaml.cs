using Event_ECS_WPF.Projects;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace Event_ECS_WPF.Controls
{
    /// <summary>
    /// Interaction logic for LoveProjectControl.xaml
    /// </summary>
    public partial class LoveProjectControl : UserControl
    {
        public LoveProjectControl()
        {
            InitializeComponent();
        }

        public LoveProject LoveProject
        {
            get { return (LoveProject)GetValue(LoveProjectProperty); }
            set { SetValue(LoveProjectProperty, value); }
        }

        public static readonly DependencyProperty LoveProjectProperty =
            DependencyProperty.Register("LoveProject", typeof(LoveProject), typeof(LoveProjectControl));

        private void SetMainFile_Click(object sender, RoutedEventArgs e)
        {
            using (Forms.OpenFileDialog dialog = new Forms.OpenFileDialog
            {
                Filter = string.Format(MainWindowViewModel.DefaultFilterFormat, "lua")
            })
            {
                switch (dialog.ShowDialog())
                {
                    case Forms.DialogResult.OK:
                        LoveProject.StartupScript = dialog.FileName;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
