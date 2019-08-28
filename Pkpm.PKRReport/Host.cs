using Serilog;
using ServiceModelEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.PKRReport
{
   internal class Host
    {
        private ServiceHost<PKRReport> _service;

        internal Host()
        {
            Log.Debug("Setting up services...");
            _service = new ServiceHost<PKRReport>(new Uri[] { });
        }

        public void Start()
        {
            Log.Debug("Starting services...");
            _service.Open();
            Log.Debug("Starting Started"); 
        }

        public void Stop()
        {
            Log.Debug("Stopping services...");
             
            try
            {
                if (_service != null)
                {
                    if (_service.State == CommunicationState.Opened)
                    {
                        _service.Close();
                    }
                }

                Log.Debug("Stopped!."); 
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not stop the service");
            }

            Log.CloseAndFlush();
        }
    }
}
