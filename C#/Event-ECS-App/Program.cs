using Event_ECS_Lib;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Event_ECS_App
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (ECS ecs = new ECS())
            {
                if(args.Length == 1)
                {
                    ecs.Initialize(File.ReadAllText(args[0]));
                }
                else if(args.Length == 3)
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
                            if (ecs.IsStarted())
                            {
                                if (ecs.CanUpdate)
                                {
                                    if (ecs.Update() == 1 && ecs.IsDisposing())
                                    {
                                        ecs.Dispose();
                                        continue;
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
            Console.Write("Press a key to end the program");
            Console.ReadKey();
        }
    }
}
