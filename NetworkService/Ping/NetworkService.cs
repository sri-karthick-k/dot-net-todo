using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        public String SendPing()
        {
            return "Success: Sent Ping!";
        }

        public int PingTimeOut(int a, int b)
        {
            return a + b;
        }

        public DateTime GetLastPing()
        {
            return DateTime.Now; 
        }
        public PingOptions GetPingOptions()
        {
            return new PingOptions() 
            { 
                Ttl = 1,
                DontFragment = true,
            };
        }

        public IEnumerable<PingOptions> GetLastPings()
        {
            IEnumerable<PingOptions> lastPings = new List<PingOptions>()
            {
                new PingOptions
                {
                    Ttl = 1,
                    DontFragment = true,
                },
                new PingOptions
                {
                    Ttl = 1,
                    DontFragment = true,
                },
                new PingOptions
                {
                    Ttl = 1,
                    DontFragment = true,
                }
            };
            return lastPings;
        }
    }
}
