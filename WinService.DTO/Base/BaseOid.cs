using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinService.DTO.Base
{
    public abstract class BaseOid
    {
        public String OID { get; set; }
        public String IP { get; set; }
        public String Nr_Serie { get; set; }
        public String Id_equipamento { get; set; }
        public String maq_contrato { get; set; }
        public String Mac_address { get; set; }
    }
}
