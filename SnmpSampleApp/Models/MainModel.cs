using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnmpSampleApp
{
    internal class MainModel
    {
        private static readonly Lazy<MainModel> _instance = new Lazy<MainModel>(() => new MainModel());

        internal static MainModel Instance => _instance.Value;
        internal SnmpManager SnmpManager { get; } = SnmpManager.Instance;
        internal ConfigData ConfigData { get; } = ConfigData.Instance;
        internal LogData LogData { get; } = new LogData();

        public MainModel()
        {
            LogData.AddLogEntry("Start...");
        }

        private SnmpModel ConnectSnmpData(string ipAddress, string community)
        {
            var snmpModel = SnmpManager.GetSnmpModel(ipAddress);
            if (snmpModel == null)
            {
                this.LogData.AddLogEntry(string.Format("Connect: ipaddr={0}, community={1}", ipAddress, community));
                snmpModel = SnmpManager.AddSnmpModel(ipAddress, community);
            }
            return snmpModel;
        }



        public string? GetSnmpData(string ipAddress, string community, string command)
        {
            var snmpModel = ConnectSnmpData(ipAddress, community);
            LogData.AddLogEntry(string.Format("Request: command='{0}'", command));
            var data = snmpModel?.GetSnmpData(command);
            LogData.AddLogEntry($"Recieve : '{data?.ResponseData ?? "null"}'");

            ConfigData.AddToListIfNotExists(ConfigData.IpAddressList, ipAddress);
            ConfigData.AddToListIfNotExists(ConfigData.CommandList, command);
            ConfigData.SaveData();
            return data?.ResponseData;
        }

        internal void GetSnmpWalk(string ipAddress, string community, string selectedCommand)
        {
            var snmpModel = ConnectSnmpData(ipAddress, community);
            LogData.AddLogEntry(string.Format("Request: command='{0}'", selectedCommand));
            var dataList = snmpModel.GetSnmpWalk(selectedCommand);
            foreach (var data in dataList)
            {
                LogData.AddLogEntry($"Recieve : {data?.Command}  '{data?.ResponseData ?? "null"}'");
            }

            ConfigData.AddToListIfNotExists(ConfigData.IpAddressList, ipAddress);
            ConfigData.AddToListIfNotExists(ConfigData.CommandList, selectedCommand);
            ConfigData.SaveData();
        }
    }
}