namespace Event_ECS_WPF.Projects.Love
{
    public class Modules : NotifyPropertyChanged
    {
        private bool _audio;
        private bool _data;
        private bool _event;
        private bool _font;
        private bool _graphics;
        private bool _image;
        private bool _joystick;
        private bool _keyboard;
        private bool _math;
        private bool _mouse;
        private bool _physics;
        private bool _sound;
        private bool _system;
        private bool _thread;
        private bool _timer;
        private bool _touch;
        private bool _video;
        private bool _window;

        public Modules()
        {
            Data = true;
            Event = true;
            Keyboard = true;
            Mouse = true;
            Timer = true;
            Joystick = true;
            Touch = true;
            Image = true;
            Graphics = true;
            Audio = true;
            Math = true;
            Physics = true;
            Sound = true;
            System = true;
            Font = true;
            Thread = true;
            Window = true;
            Video = true;
        }

        public bool Audio { get => _audio; set { _audio = value; OnPropertyChanged(); } }
        public bool Data { get => _data; set { _data = value; OnPropertyChanged(); } }
        public bool Event { get => _event; set { _event = value; OnPropertyChanged(); } }
        public bool Font { get => _font; set { _font = value; OnPropertyChanged(); } }
        public bool Graphics { get => _graphics; set { _graphics = value; OnPropertyChanged(); } }
        public bool Image { get => _image; set { _image = value; OnPropertyChanged(); } }
        public bool Joystick { get => _joystick; set { _joystick = value; OnPropertyChanged(); } }
        public bool Keyboard { get => _keyboard; set { _keyboard = value; OnPropertyChanged(); } }
        public bool Math { get => _math; set { _math = value; OnPropertyChanged(); } }
        public bool Mouse { get => _mouse; set { _mouse = value; OnPropertyChanged(); } }
        public bool Physics { get => _physics; set { _physics = value; OnPropertyChanged(); } }
        public bool Sound { get => _sound; set { _sound = value; OnPropertyChanged(); } }
        public bool System { get => _system; set { _system = value; OnPropertyChanged(); } }
        public bool Thread { get => _thread; set { _thread = value; OnPropertyChanged(); } }
        public bool Timer { get => _timer; set { _timer = value; OnPropertyChanged(); } }
        public bool Touch { get => _touch; set { _touch = value; OnPropertyChanged(); } }
        public bool Video { get => _video; set { _video = value; OnPropertyChanged(); } }
        public bool Window { get => _window; set { _window = value; OnPropertyChanged(); } }
    }
}
