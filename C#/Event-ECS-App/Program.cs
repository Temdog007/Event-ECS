using Event_ECS_Lib;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Event_ECS_App
{
    class Program
    {
        public ManualResetEvent StartEvent { get; } = new ManualResetEvent(false);

        public AutoResetEvent UpdateEvent { get; } = new AutoResetEvent(false);

        public ManualResetEvent DisposeEvent { get; } = new ManualResetEvent(false);

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start(args);

            Console.Write("Press a key to end the program");
            Console.ReadKey();
        }

        void Start(string[] args)
        {
            using (ECS ecs = new ECS())
            {
                ecs.Starting += Ecs_Starting;
                ecs.Disposing += Ecs_Disposing;
                ecs.Updated += Ecs_Updated;

                if (args.Length == 1)
                {
                    ecs.Initialize(File.ReadAllText(args[0]));
                }
                else if (args.Length == 3)
                {
                    ecs.Initialize(File.ReadAllText(args[0]), args[1], args[2]);
                }

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

        private void Ecs_Updated(int obj)
        {
            if (obj == 2)
            {
                UpdateEvent.Set();
            }
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
    }
}
