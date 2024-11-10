using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Net;

namespace SnmpSampleApp
{
    public class SnmpModel
    {
        private string _ipAddress;
        private string _community;
        private int _port;

        public SnmpModel(string ipAddress, string community, int port = 161)
        {
            _ipAddress = ipAddress;
            _community = community;
            _port = port;
        }

        public string GetSnmpData(string oid)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
                var community = new OctetString(_community);
                var result = Messenger.Get(VersionCode.V1, endpoint, community, new List<Variable> { new Variable(new ObjectIdentifier(oid)) }, 6000);

                if (result.Count > 0 && result[0].Data.TypeCode != SnmpType.NoSuchObject)
                {
                    return result[0].Data.ToString();
                }
                else
                {
                    throw new Exception("SNMPリクエストが失敗しました。");
                }
            }
            catch (Exception ex)
            {
                return $"エラー: {ex.Message}";
            }
        }
    }
}
