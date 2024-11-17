using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnmpSampleApp.Data
{
    public class SnmpResponse
    {
        public bool Success { get; set; }
        public string IpAddress { get; set; }
        public string Command { get; set; }
        public string ResponseData { get; set; }
        public DateTime Timestamp { get; set; }

        public SnmpResponse(string ipAddress, string command, string responseData, DateTime? timestamp = null)
        {
            Success = true;
            IpAddress = ipAddress;
            Command = command;
            ResponseData = responseData;
            Timestamp = timestamp?? DateTime.Now;
        }
        public SnmpResponse(bool success, string ipAddress, string command, string msg, DateTime? timestamp = null)
        {
            Success = success;
            IpAddress = ipAddress;
            Command = command;
            ResponseData = msg;
        }

        public string Log()
        {
            return $"{Timestamp} : {Command} : {ResponseData}";
        }
    }
}
