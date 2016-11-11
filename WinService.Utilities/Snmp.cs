using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using System.Net.NetworkInformation;
using Mono.Options;
using WinService.DTO;
namespace WinService.Utilities
{
    public class Snmp
    {
        public OidDTO capturaOID(String Ip, String Oid)
        {
            OidDTO dispositivoDTO = new OidDTO();

          

     //       bool pingable = false;
    //        Ping pinger = new Ping();

  //          PingReply reply = pinger.Send(Ip);
  //          pingable = reply.Status == IPStatus.Success;

          //  if (pingable)
        //    {



                try
                {

                    var result = Messenger.Get(VersionCode.V1,
                                        new IPEndPoint(IPAddress.Parse(Ip), 161),
                                        new OctetString("public"),
                                        new List<Variable> { new Variable(new ObjectIdentifier(Oid)) },
                                        1200);
                    var valor = result[0].Data.ToString();
                    var id = result[0].Id.ToString();
                    dispositivoDTO.Valor = valor;
                    dispositivoDTO.Id = id;
                    dispositivoDTO.Valor_Byte = AddressBytesToString(result[0].Data.ToBytes());

                }
                catch (Exception )
                {
                   // throw new Exception("Erro ao Inserir Modelo " + ex.Message);
                    dispositivoDTO.Valor = "Nulo";

                }



          //  }
       //     else
    //        {

      //          dispositivoDTO.Valor = "Nulo";
    //        }



            return dispositivoDTO;



        }

        private static string AddressBytesToString(byte[] addressBytes)
        {
            return string.Join(":", (from b in addressBytes
                                     select b.ToString("X2")).ToArray());
        }
    }
}
