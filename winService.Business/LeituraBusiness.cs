using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.Repository;

namespace WinService.Business
{
    public class LeituraBusiness
    {

        EquipamentosBusiness _BLLEquipamentos = new EquipamentosBusiness();


        public void Insere_Leitura(IEnumerable<OIDs_Dispositivo> Lista_Dispositivo)
        {

        }

       
        public IEnumerable<OIDs_Dispositivo> Captura_Leitura_Na_Rede()
        {
            //Regra: caso seja um dispositvo valido (Impressora), verifica se existe na lista de equipamentos
            //Se for um equipamento de contrato busca modelo/Matriz, se não existir na lista cria com equipamento de outro
            //usa a lista/matriz generica

            var Lista_dispositivo_Encontrados = _BLLEquipamentos.Captura_Ip_Dipositivos_Na_rede();

            var Lista_OIDs_Capturados = _BLLEquipamentos.Captura_OID_Dispositivo(Lista_dispositivo_Encontrados);

            return Lista_OIDs_Capturados;

        }
    }
}
