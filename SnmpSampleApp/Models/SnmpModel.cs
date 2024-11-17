using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using SnmpSampleApp.Data;
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

        public Data.SnmpResponse GetSnmpData(string oid)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
                var community = new OctetString(_community);
                var result = Messenger.Get(VersionCode.V1, endpoint, community, new List<Variable> { new Variable(new ObjectIdentifier(oid)) }, 6000);

                if (result.Count > 0 && result[0].Data.TypeCode != SnmpType.NoSuchObject)
                {
                    var response = new Data.SnmpResponse(_ipAddress, oid, result[0].Data.ToString());
                    return response;
                }
                else
                {
                    throw new Exception("SNMPリクエストが失敗しました。");
                }
            }
            catch (Exception ex)
            {
                var response = new Data.SnmpResponse(false, _ipAddress, oid, $"エラー: {ex.Message}");
                return response;
            }
        }

        internal List<Data.SnmpResponse> GetSnmpWalk(string selectedCommand)
        {
            var responses = new List<Data.SnmpResponse>();
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
                var community = new OctetString(_community);
                var variables = new List<Variable>();
                var tableOid = new ObjectIdentifier(selectedCommand);

                Messenger.Walk(VersionCode.V1, endpoint, community, tableOid, variables, 6000, WalkMode.WithinSubtree);

                foreach (var variable in variables)
                {
                    var response = new Data.SnmpResponse(_ipAddress, variable.Id.ToString(), variable.Data.ToString());
                    responses.Add(response);
                }
            }
            catch (Exception ex)
            {
                var response = new Data.SnmpResponse(false, _ipAddress, selectedCommand, $"エラー: {ex.Message}");
                responses.Add(response);
            }

            return responses;
        }
    }
}
