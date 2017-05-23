using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinService.DTO
{
    public class ConfiguracaoSistemaDTO
    {
        public Int32 Intervalo_captura { get; set; }
        

    }

    public class ConfiguracaoDTO
    {
        public Int32 Id { get; set; }
        public String Id_Cliente { get; set; }
        public Int32 Ciclo_Leitura { get; set; }
        public Int32 Ciclo_UpLoad { get; set; }
        public String Ip_Pc { get; set; }
        public String Range_Ip { get; set; }
        public String Url_Ws { get; set; }
        public String Nr_Contrato { get; set; }


    }
}
