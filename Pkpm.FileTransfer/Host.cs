 using ServiceModelEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Pkpm.FileTransfer
{
    internal class Host
    {
        private ServiceHost<FileTransfer> _service;

        internal Host()
        {
            Console.WriteLine("Setting up services...");
            _service = new ServiceHost<FileTransfer>(new Uri[] { });
        }

        public void Start()
        {
            Console.WriteLine("Starting services...");
            _service.Open();
            Console.WriteLine("Started!");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping services...");
            try
            {
                if (_service != null)
                {
                    if (_service.State == CommunicationState.Opened)
                    {
                        _service.Close();
                    }
                }
                Console.WriteLine("Stopped!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not stop: " + ex.Message);
            }
        }
    }
}
