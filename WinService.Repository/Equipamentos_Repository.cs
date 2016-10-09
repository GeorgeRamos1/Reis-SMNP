using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;

namespace WinService.Repository
{
    public class Equipamentos_Repository
    {

        public IEnumerable<OIDs_Dispositivo> Lista_OID_Dispositio_Rep()
        {


            var itens = this.getOIDsDispositivo();

            return itens;
        }


        public IEnumerable<OIDs_Dispositivo> getOIDsDispositivo()
        {
            var Oid1 = new OIDs_Dispositivo { IP = "192.168.100.1", OID = new Guid("681F2089-87B1-42F4-B8CC-66EDD470C773"),Descricao="Contador A4 PB", Hexa="00" };

            var Oid2 = new OIDs_Dispositivo { IP = "192.168.100.2", OID = new Guid("D3F47F3D-567A-4802-847C-F75B0D1C5DBB"), Descricao = "Contador A3 PB", Hexa = "00" };

            var lista = new List<OIDs_Dispositivo>();
            lista.Add(Oid1);
            lista.Add(Oid2);


            return lista;

        }





    }
}
