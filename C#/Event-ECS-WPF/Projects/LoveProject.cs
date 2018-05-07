using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Event_ECS_WPF.Projects
{
    public enum FullscreenType
    {
        DESKTOP,
        EXCLUSIVE
    }

    public enum UpdateType
    {
        MANUAL,
        AUTOMATIC
    }

    [XmlRoot("LoveProject")]
    public class LoveProject : Project
    {
        private const string confFormat =
@"return function(t)
    t.accelerometerjoystick = {0}      -- Enable the accelerometer on iOS and Android by exposing it as a Joystick(boolean)
    t.externalstorage = {1}           -- True to save files(and read from the save directory) in external storage on Android(boolean)
    t.gammacorrect = {2}             -- Enable gamma-correct rendering, when supported by the system(boolean)

    t.audio.mixwithsystem = {3}       -- Keep background music playing when opening LOVE(boolean, iOS and Android only)

    t.window.width = {4}                -- The window width(number)
    t.window.height = {5}               -- The window height(number)
    t.window.borderless = {6}         -- Remove all border visuals from the window(boolean)
    t.window.resizable = {7}          -- Let the window be user-resizable(boolean)
    t.window.minwidth = {8}               -- Minimum window width if the window is resizable(number)
    t.window.minheight = {9}             -- Minimum window height if the window is resizable(number)
    t.window.fullscreen = {10}         -- Enable fullscreen(boolean)
    t.window.fullscreentype = {11} -- Choose between 'desktop' fullscreen or 'exclusive' fullscreen mode(string)
    t.window.vsync = {12}             -- Vertical sync mode(number)
    t.window.msaa = {13}                  -- The number of samples to use with multi-sampled antialiasing(number)
    t.window.display = {14}                -- Index of the monitor to show the window in (number)
    t.window.highdpi = {15}            -- Enable high-dpi mode for the window on a Retina display (boolean)

    t.modules.audio = {16}-- Enable the audio module (boolean)
    t.modules.data = {17}-- Enable the data module (boolean)
    t.modules.event = {18}              -- Enable the event module (boolean)
    t.modules.font = {19}               -- Enable the font module (boolean)
    t.modules.graphics = {20}-- Enable the graphics module (boolean)
    t.modules.image = {21}-- Enable the image module (boolean)
    t.modules.joystick = {22}-- Enable the joystick module (boolean)
    t.modules.keyboard = {23}-- Enable the keyboard module (boolean)
    t.modules.math = {24}-- Enable the math module (boolean)
    t.modules.mouse = {25}-- Enable the mouse module (boolean)
    t.modules.physics = {26}-- Enable the physics module (boolean)
    t.modules.sound = {27}-- Enable the sound module (boolean)
    t.modules.system = {28}-- Enable the system module (boolean)
    t.modules.thread = {29}-- Enable the thread module (boolean)
    t.modules.timer = {30}-- Enable the timer module (boolean), Disabling it will result 0 delta time in love.update
    t.modules.touch = {31}-- Enable the touch module (boolean)
    t.modules.video = {32}-- Enable the video module (boolean)
    t.modules.window = {33}-- Enable the window module (boolean)
end";

        private LoveProjectSettings _settings;
        private UpdateType _updateType;
        public LoveProject()
        {
            Settings = new LoveProjectSettings();
            UpdateType = UpdateType.MANUAL;
        }

        [XmlElement("Settings")]
        public LoveProjectSettings Settings { get => _settings; set { _settings = value; OnPropertyChanged(); } }

        public override ProjectType Type => ProjectType.LOVE;

        [XmlElement]
        public UpdateType UpdateType
        {
            get => _updateType;
            set
            {
                _updateType = value;
                OnPropertyChanged("UpdateType");
            }
        }

        public override void Start()
        {
            File.WriteAllText(Path.Combine(Assembly.GetExecutingAssembly().Location, "conf.lua"), string.Format(confFormat,
                Settings.AccelerometerJoystick, Settings.ExternalStorage, Settings.GammaCorrect, Settings.MixWithSystem, Settings.Width, Settings.Height,
                Settings.Borderless, Settings.Resizable, Settings.MinWidth, Settings.MinHeight, Settings.Fullscreen, Settings.FullscreenType.ToString().ToLower(),
                Settings.VSync, Settings.Msaa, Settings.Display, Settings.HighDPI, Settings.Modules["Audio"], Settings.Modules["Data"], Settings.Modules["Event"],
                Settings.Modules["Font"], Settings.Modules["Graphics"], Settings.Modules["Image"], Settings.Modules["Joystick"], Settings.Modules["keyboard"], Settings.Modules["Math"],
                Settings.Modules["Mouse"], Settings.Modules["Physics"], Settings.Modules["Sound"], Settings.Modules["System"], Settings.Modules["Thread"], Settings.Modules["Timer"],
                Settings.Modules["Touch"], Settings.Modules["Video"], Settings.Modules["Window"]));
            base.Start();
        }
    }

    public class LoveProjectSettings : NotifyPropertyChanged
    {
        private bool _accelerometerJoystick;
        private bool _borderless;
        private bool _centered;
        private int _display;
        private bool _externalStorage;
        private bool _fullscreen;
        private FullscreenType _fullscreenType;
        private bool _gammaCorrect;
        private int _height;
        private bool _highDPI;
        private int _minHeight;
        private int _minWidth;
        private bool _mixWithSystem;
        private Modules _modules;
        private int _msaa;
        private bool _resizable;
        private int _vSync;
        private int _width;

        public LoveProjectSettings()
        {
            Width = 800;
            Height = 600;

            MinWidth = 1;
            MinHeight = 1;

            Fullscreen = false;
            FullscreenType = FullscreenType.DESKTOP;
            Display = 1;
            VSync = 1;
            Msaa = 1;

            Borderless = false;
            Resizable = false;
            Centered = true;
            HighDPI = false;

            Modules = new Modules();
        }

        public bool AccelerometerJoystick { get => _accelerometerJoystick; set { _accelerometerJoystick = value; OnPropertyChanged(); } }
        public bool Borderless { get => _borderless; set { _borderless = value; OnPropertyChanged(); } }
        public bool Centered { get => _centered; set { _centered = value; OnPropertyChanged(); } }
        public int Display { get => _display; set { _display = value; OnPropertyChanged(); } }
        public bool ExternalStorage { get => _externalStorage; set { _externalStorage = value; OnPropertyChanged(); } }
        public bool Fullscreen { get => _fullscreen; set { _fullscreen = value; OnPropertyChanged(); } }
        public FullscreenType FullscreenType { get => _fullscreenType; set { _fullscreenType = value; OnPropertyChanged(); } }
        public bool GammaCorrect { get => _gammaCorrect; set { _gammaCorrect = value; OnPropertyChanged(); } }
        public int Height { get => _height; set { _height = value; OnPropertyChanged(); } }
        public bool HighDPI { get => _highDPI; set { _highDPI = value; OnPropertyChanged(); } }
        public int MinHeight { get => _minHeight; set { _minHeight = value; OnPropertyChanged(); } }
        public int MinWidth { get => _minWidth; set { _minWidth = value; OnPropertyChanged(); } }
        public bool MixWithSystem { get => _mixWithSystem; set { _mixWithSystem = value; OnPropertyChanged(); } }
        public Modules Modules { get => _modules; set { _modules = value; OnPropertyChanged(); } }
        public int Msaa { get => _msaa; set { _msaa = value; OnPropertyChanged(); } }
        public bool Resizable { get => _resizable; set { _resizable = value; OnPropertyChanged(); } }
        public int VSync { get => _vSync; set { _vSync = value; OnPropertyChanged(); } }
        public int Width { get => _width; set { _width = value; OnPropertyChanged(); } }
    }

    public class Modules : IEnumerable<Module>
    {
        private static readonly string[] modNames =
        {
            "Data",
            "Event",
            "Keyboard",
            "Mouse",
            "Timer",
            "Joystick",
            "Touch",
            "Image",
           " Graphics",
            "Audio",
           " Math",
           " Physics",
            "Sound",
            "System",
            "Font",
            "Thread",
            "Window",
           " Video"
        };

        private readonly Module[] modules;

        public Modules()
        {
            List<Module> mods = new List<Module>();
            foreach (var modeName in modNames)
            {
                mods.Add(new Module(modeName));
            }
            modules = mods.ToArray();
        }

        public bool this[string key] => modules.Single(m => m.Name == key)?.Enabled ?? false;

        public IEnumerator<Module> GetEnumerator()
        {
            return ((IEnumerable<Module>)modules).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Module>)modules).GetEnumerator();
        }
    }
}
