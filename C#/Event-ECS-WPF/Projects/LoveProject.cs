using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Projects.Love;
using Event_ECS_WPF.SystemObjects;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{
    [XmlRoot("LoveProject")]
    public class LoveProject : Project
    {
        private LoveProjectSettings _settings;

        public LoveProject() : this(false) { }

        public LoveProject(bool setDefaults) : base(setDefaults)
        {
            Settings = new LoveProjectSettings();
        }

        [XmlElement("Settings")]
        public LoveProjectSettings Settings { get => _settings; set { _settings = value; OnPropertyChanged(); } }

        public override ProjectType Type => ProjectType.LOVE;

        public override bool Start()
        {
            string text = string.Format("Event_ECS_WPF.Lua.conf.lua".GetResourceFileContents(),
            Settings.AccelerometerJoystick, Settings.ExternalStorage, Settings.GammaCorrect, Settings.MixWithSystem, Settings.Width, Settings.Height,
            Settings.Borderless, Settings.Resizable, Settings.MinWidth, Settings.MinHeight, Settings.Fullscreen, Settings.FullscreenType.ToString().ToLower(),
            Settings.VSync, Settings.Msaa, Settings.Display, Settings.HighDPI, Settings.Modules.Audio, Settings.Modules.Data, Settings.Modules.Event,
            Settings.Modules.Font, Settings.Modules.Graphics, Settings.Modules.Image, Settings.Modules.Joystick, Settings.Modules.Keyboard, Settings.Modules.Math,
            Settings.Modules.Mouse, Settings.Modules.Physics, Settings.Modules.Sound, Settings.Modules.System, Settings.Modules.Thread, Settings.Modules.Timer,
            Settings.Modules.Touch, Settings.Modules.Video, Settings.Modules.Window, Name);

            text = text.Replace("True", "true").Replace("False", "false");
            File.WriteAllText(Path.Combine(OutputPath, "conf.lua"), text);
            File.WriteAllText(Path.Combine(OutputPath, "main.lua"), "Event_ECS_WPF.Lua.main.lua".GetResourceFileContents());

            return base.Start();
        }

        protected override void StartApplication()
        {
            if (!ECS.Instance.TargetAppIsRunning)
            {
                Process.Start(StartInfo);
                ECS.Instance.AppName = "love";
            }
        }

        public override ProcessStartInfo StartInfo
        {
            get
            {
                return new ProcessStartInfo()
                {
                    FileName = Properties.Settings.Default.Love2D,

                    UseShellExecute = false,
#if DEBUG
                    Arguments = string.Join(" ", string.Format("\"{0}\"", OutputPath), "DEBUG_MODE")
#else
                    Arguments = string.Join(" ", string.Format("\"{0}\"", OutputPath))
#endif
                };
            }
        }
    }
}
