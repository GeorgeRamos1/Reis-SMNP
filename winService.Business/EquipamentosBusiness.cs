using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.Repository;
using WinService.Utilities;

namespace WinService.Business
{
    public class EquipamentosBusiness
    {

        Equipamentos_Repository _repositoryDispositivo = new Equipamentos_Repository();
        Leitura_Repository _repositoryLeitura = new Leitura_Repository();
        Snmp _SNMP = new Snmp();
        
        //Busca os Ip que estão respondendo na rede como impressora
        public IEnumerable<EquipamentosDTO>Captura_Ip_Dipositivos_Na_rede()
        {
            //Captura o range de IP
            String vRange = "10.0.0.";
            String vOid_Serial = "1.3.6.1.2.1.43.5.1.1.17.1";
            String vOid_Mac = "1.3.6.1.2.1.2.2.1.6.1";
          //  String vOid_Model = "1.3.6.1.2.1.43.5.1.1.16.1";
            String vOid_Model = "1.3.6.1.2.1.25.3.2.1.3.1";
            String vOid_prn = "1.3.6.1.2.1.43.5.1.1.1.1";
            String vIp;
            var lista_Dispostivos_encontrados = new List<EquipamentosDTO>();

            var dt_inicio = DateTime.Now;
            for (int i = 30; i < 255; i++)
            {

                vIp = vRange + i.ToString();

                var result = _SNMP.capturaOID(vIp, vOid_prn);

                if (String.IsNullOrEmpty(result.Id)==false)
                {
                    var lista_pesq = new EquipamentosDTO();

                    var vNrSerie = _SNMP.capturaOID(vIp, vOid_Serial);
                    var vMac = _SNMP.capturaOID(vIp, vOid_Mac);
                    var VModel = _SNMP.capturaOID(vIp, vOid_Model);
                    
                    lista_pesq.IP = vIp;
                    lista_pesq.Nr_Serie = vNrSerie.Valor;
                    lista_pesq.MAcAdress = vMac.Valor;
                    lista_pesq.Modelo = VModel.Valor;


                    lista_Dispostivos_encontrados.Add(lista_pesq);
                }

            }
            var dt_Fim = DateTime.Now;

            var Tempo_loop = dt_Fim - dt_inicio;

            return lista_Dispostivos_encontrados;
        }

        // Para a lista de dispositivos encontrados buscar o grupo de OIDs Correspondentes para leitura
        public IEnumerable<OIDs_Dispositivo> Captura_OID_Dispositivo(IEnumerable<EquipamentosDTO> Lista_Dispositivo)
        {

            var lista_OID_Todos_Dispositivos = new List<OIDs_Dispositivo>();

            foreach (var Dispositivo in Lista_Dispositivo)
            {
                // PEsquisa se o Dispositivo Existe na lista de Equipamentos/Matriz/Modelo e se pertence a Empresa
                //Se não existir utiliza-se da matriz padrão

                var Lista_OIDs = _repositoryDispositivo.getOIDsDispositivo(Dispositivo.IP, Dispositivo.Nr_Serie);

                //Caso não seja encontrado nenhum registro, criar com  OID padrão para leitura.
                if (Lista_OIDs.Count()==0)
                {
                    Lista_OIDs = new List<OIDs_Dispositivo> { new OIDs_Dispositivo { IP = Dispositivo.IP, Nr_Serie = Dispositivo.Nr_Serie, OID = "1.3.6.1.2.1.43.10.2.1.4.1.1" } };
                }
           
                lista_OID_Todos_Dispositivos.AddRange(Lista_OIDs);
            }


            return lista_OID_Todos_Dispositivos;

        }




    }
}
