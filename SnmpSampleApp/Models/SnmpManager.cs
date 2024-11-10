using SnmpSampleApp;
using System;
using System.Collections.Generic;

namespace SnmpSampleApp
{
    public class SnmpManager
    {
        private static readonly Lazy<SnmpManager> _instance = new Lazy<SnmpManager>(() => new SnmpManager());
        private Dictionary<string, SnmpModel> _snmpModels;

        private SnmpManager()
        {
            _snmpModels = new Dictionary<string, SnmpModel>();
        }

        internal static SnmpManager Instance => _instance.Value;

        internal SnmpModel AddSnmpModel(string ipAddress, string community, int port = 161)
        {
            if (!_snmpModels.ContainsKey(ipAddress))
            {
                _snmpModels[ipAddress] = new SnmpModel(ipAddress, community, port);
            }
            return _snmpModels[ipAddress];
        }

        internal SnmpModel? GetSnmpModel(string ipAddress)
        {
            return _snmpModels.ContainsKey(ipAddress) ? _snmpModels[ipAddress] : null;
        }

        internal bool RemoveSnmpModel(string ipAddress)
        {
            return _snmpModels.Remove(ipAddress);
        }
    }
}