using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public class WcfProxy<TContract> : IDisposable
         where TContract : class
    {
        public TContract Service { get; private set; }

        public WcfProxy()
        {
            try
            {
                var factory = new ChannelFactory<TContract>(typeof(TContract).Name + "_Endpoint");
                factory.Open();
                Service = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not create proxy: {0}", ex.Message);
                Service = null;
            }
        }

        public void Dispose()
        {
            if (Service != null)
            {
                var internalProxy = Service as ICommunicationObject;

                try
                {
                    if (internalProxy != null)
                    {
                        if (internalProxy.State != CommunicationState.Closed && internalProxy.State != CommunicationState.Faulted)
                            internalProxy.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not close proxy: {0}", ex.Message);
                    try
                    {
                        if (internalProxy != null)
                            internalProxy.Abort();
                    }
                    catch (Exception exInternal)
                    {
                        Console.WriteLine("Could not abort proxy: {0}", exInternal.Message);
                    }
                }

                if (internalProxy is IDisposable)
                {
                    try
                    {
                        if (internalProxy.State != CommunicationState.Faulted)
                            (internalProxy as IDisposable).Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not dispose proxy: ", ex.Message);
                    }
                }
            }
        }
    }
}
