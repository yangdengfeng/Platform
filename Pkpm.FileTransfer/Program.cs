using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Pkpm.FileTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FileTransfer Service");
            try
            {
                const string name = "FileTransfer-Service";
                const string description = "FileTransfer Service";
                var host = HostFactory.New(configuration =>
                {
                    configuration.Service<Host>(callback =>
                    {
                        callback.ConstructUsing(s => new Host());
                        callback.WhenStarted(service => service.Start());
                        callback.WhenStopped(service => service.Stop());
                    });
                    configuration.SetDisplayName(name);
                    configuration.SetServiceName(name);
                    configuration.SetDescription(description);
                    configuration.RunAsLocalService();
                });
                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FileTransfer Service fatal exception. " + ex.Message);
            }

            Console.ReadLine();

        }
    }
}
