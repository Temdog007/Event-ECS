namespace Event_ECS_WPF.Projects.Love
{
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
}
