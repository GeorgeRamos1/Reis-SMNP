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
            String vRange = "192.168.1.";
            String vOid_Serial = "1.3.6.1.2.1.43.5.1.1.17.1";
            String vIp;
            var lista_Dispostivos_encontrados = new List<EquipamentosDTO>();

            var dt_inicio = DateTime.Now;
            for (int i = 2; i < 255; i++)
            {

                vIp = vRange + i.ToString();

                var result = _SNMP.capturaOID(vIp, vOid_Serial);

                if (String.IsNullOrEmpty(result.Id)==false)
                {
                    var lista_pesq = new EquipamentosDTO();
                    lista_pesq.IP = vIp;
                    lista_pesq.Nr_Serie = result.Valor;

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

            foreach (var Dispositivo in Lista_Dispositivo)
            {
                // PEsquisa se o Dispositivo Existe na lista de Equipamentos/Matriz/Modelo e se pertence a Empresa
                //Se não existir utiliza-se da matriz padrão




                
            }
            
            
            return _repositoryDispositivo.Lista_OID_Dispositio_Rep();

        }




    }
}
