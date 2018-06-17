using Event_ECS_Lib;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Event_ECS_App
{
    class Program : IDisposable
    {
        readonly ECS ecs;

        readonly Timer timer;

        Program()
        {
            ecs = new ECS();
            ecs.Starting += Ecs_Starting;
            ecs.Disposing += Ecs_Disposing;
            ecs.Updated += Ecs_Updated;

            timer = new Timer(OnTimer, ecs, 0, 1000);
        }

        public ManualResetEvent DisposeEvent { get; } = new ManualResetEvent(false);

        public ManualResetEvent StartEvent { get; } = new ManualResetEvent(false);

        public AutoResetEvent UpdateEvent { get; } = new AutoResetEvent(false);

        public void Dispose()
        {
            ecs.Dispose();
            timer.Dispose();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                if (args.Length == 1)
                {
                    program.ecs.Initialize(File.ReadAllText(args[0]));
                }
                else if (args.Length == 3)
                {
                    program.ecs.Initialize(File.ReadAllText(args[0]), args[1], args[2]);
                }

                program.Start();
            }

            Console.Write("Press a key to end the program");
            Console.ReadKey();
        }

        private void Ecs_Disposing()
        {
            DisposeEvent.Set();
            StartEvent.Reset();
        }

        private void Ecs_Starting()
        {
            StartEvent.Set();
            DisposeEvent.Reset();
        }

        private void Ecs_Updated(int obj)
        {
            if (obj == 1)
            {
                UpdateEvent.Set();
            }
        }

        private static void OnTimer(object state)
        {
            if (state is ECS ecs)
            {
                ecs.UpdateClient();
            }
        }

        void Start()
        {
            using (ServiceHost serviceHost = new ServiceHost(ecs))
            {
                serviceHost.OpenTimeout = TimeSpan.FromMinutes(5);
                serviceHost.CloseTimeout = TimeSpan.FromMinutes(5);

                serviceHost.Closed += (o, e) => Console.WriteLine("Service host closed");
                serviceHost.Closing += (o, e) => Console.WriteLine("Service host closing");
                serviceHost.Faulted += (o, e) => Console.WriteLine("Service host faulted");
                serviceHost.Opened += (o, e) => Console.WriteLine("Service host opened");
                serviceHost.Opening += (o, e) => Console.WriteLine("Service host opening");
                serviceHost.UnknownMessageReceived += (o, e) => Console.WriteLine(e.Message);

                try
                {
                    NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                    serviceHost.AddServiceEndpoint(typeof(IECSWrapper), binding, ECSWrapperValues.ECSAddress);
                    serviceHost.Open();

                    while (true)
                    {
                        if (StartEvent.WaitOne(100))
                        {
                            if (ecs.CanUpdate)
                            {
                                ecs.Update();
                                if (!UpdateEvent.WaitOne(100) || DisposeEvent.WaitOne(1))
                                {
                                    ecs.Dispose();
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    serviceHost.Abort();
                    Console.WriteLine(e);
                    Environment.ExitCode = -1;
                }
            }
        }
    }
}
