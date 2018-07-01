using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Projects.Love;
using Event_ECS_WPF.SystemObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{
    [XmlRoot("LoveProject")]
    public class LoveProject : Project
    {
        public const string LoadEventECS = "require('eventecs')";

        public const string LoadLoveRun = "require('eventecsloverun')\nrequire('loveRun')";

        public const string LoadEventECSServer = "require('eventecsserver')";

        public const string LoadServerEntity = "require('serverSystem') -- Remove this when releasing game";

        private LoveProjectSettings _settings;

        private string _startupScripts = string.Empty;

        public LoveProject() : this(false) { }

        public LoveProject(bool setDefaults) : base(setDefaults)
        {
            Settings = new LoveProjectSettings();
        }

        [XmlElement("Settings")]
        public LoveProjectSettings Settings { get => _settings; set { _settings = value; OnPropertyChanged(); } }
        
        [XmlElement]
        public string StartupScript
        {
            get => _startupScripts;
            set
            {
                _startupScripts = value;
                OnPropertyChanged("StartupScript");
            }
        }

        public override ProjectType Type => ProjectType.LOVE;

        public override bool Start()
        {
            return CheckOutDir() &&  CopyLibraries() && CopyLuaFiles() && StartApplication();
        }

        private bool CopyLuaFiles()
        {
            try
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
                File.WriteAllText(Path.Combine(OutputPath, "main.lua"), string.Join(Environment.NewLine, LoadEventECS, LoadLoveRun, 
                                                    LoadEventECSServer,  LoadServerEntity,
                                                    Environment.NewLine, File.ReadAllText(StartupScript)));

                return true;
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(e);
                return false;
            }
        }

        private bool CopyLibraries()
        {
            try
            {
                foreach (var libraryPath in LibraryPaths.Select(l => l.Value))
                {
                    if (!libraryPath.IsHidden() && Directory.Exists(libraryPath))
                    {
                        foreach (var file in Directory.GetFiles(libraryPath).Where(f => !f.IsHidden() && Path.GetExtension(f) == ".dll"))
                        {
                            string dest = Path.Combine(OutputPath, Path.GetFileName(file));
                            FileInfo f1 = new FileInfo(file);
                            FileInfo f2 = new FileInfo(dest);
                            if (!f2.Exists || f1.Length != f2.Length || f1.LastWriteTime != f2.LastWriteTime)
                            {
                                File.Copy(file, dest, true);
                                f2.LastWriteTime = f1.LastWriteTime;
                                LogManager.Instance.Add(LogLevel.Medium, "Copied {0} to {1}", file, dest);
                            }
                        }
                    }
                }

                CopyComponentsToOutputPath();
                return true;
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
                return false;
            }
        }

        private bool StartApplication()
        {
            try
            {
                if (!ECS.Instance.TargetAppIsRunning)
                {
                    Process.Start(StartInfo);
                    ECS.Instance.AppName = "love";
                }
                return true;
            }
            catch(Exception e)
            {
                LogManager.Instance.Add(e);
                return false;
            }
        }

        public ProcessStartInfo StartInfo
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
