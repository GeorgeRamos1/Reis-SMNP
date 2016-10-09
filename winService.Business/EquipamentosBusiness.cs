using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.Repository;

namespace WinService.Business
{
    public class EquipamentosBusiness
    {

        Equipamentos_Repository _repositoryDispositivo = new Equipamentos_Repository();
        Leitura_Repository _repositoryLeitura = new Leitura_Repository();
 
        
        //Busca os Ip que estão respondendo na rede como impressora
        public IEnumerable<Equipamentos>Captura_Ip_Dipositivos_Na_rede()
        {
            return null;
        }

        // Para a lista de dispositivos encontrados buscar o grupo de OIDs Correspondentes para leitura
        public IEnumerable<OIDs_Dispositivo> Captura_OID_Dispositivo(IEnumerable<Equipamentos> Lista_Dispositivo)
        {
            return _repositoryDispositivo.Lista_OID_Dispositio_Rep();

        }


// mudar para leitura
        public void Captura_Leitura_Na_Rede()
        {
            //Regra: caso seja um dispositvo valido (Impressora), verifica se existe na lista de equipamentos
            //Se for um equipamento de contrato busca modelo/Matriz, se não existir na lista cria com equipamento de outro
            //usa a lista/matriz generica

            var Lista_dispositivo_Encontrados = Captura_Ip_Dipositivos_Na_rede();

            var Lista_Dispositivos_OID = Captura_OID_Dispositivo(Lista_dispositivo_Encontrados);



        }

    }
}
