using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinService.DTO
{
    public class OID_Leitura: Base.BaseOid
    {

        public String Id_Cliente { get; set; }
        public DateTime Dt_leitura { get; set; }
        public DateTime? Dt_Transmissao { get; set; }
        public String Id_Equipamento { get; set; }
        public String Valor { get; set; }
    }
}
