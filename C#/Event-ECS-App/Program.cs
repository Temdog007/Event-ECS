using Event_ECS_Lib;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Event_ECS_App
{
    class Program : IDisposable
    {
        readonly ECS ecs = new ECS();

        public void Dispose()
        {
            ecs.Uninitialize();
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

                    int update = 0;
                    while (true)
                    {
                        if (ecs.HasStarted)
                        {
                            if (ecs.CanUpdate)
                            {
                                ecs.LoveUpdate(out update);
                                if (update != 1 || ecs.DisposeRequested)
                                {
                                    ecs.Stop();
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(100);
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
