using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace DCMGToolbox
{
    public class Network
    {
        public bool Pingable(string iporhostname)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(iporhostname);
                pingable = reply.Status == IPStatus.Success;
            }
            catch(NullReferenceException)
            {
                pingable = false;
            }

            return pingable;
        }
    }
}
