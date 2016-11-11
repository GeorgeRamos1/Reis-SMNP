using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinService.DTO
{
    public class Global
    {
        public static Int32 Ciclo { get; set; }
        public static String Faixa_Ip { get; set; }
        public static Int32 Inicio_faixa { get; set; }
        public static Int32 Fim_faixa { get; set; }
        public static String Oid_contador_geral { get; set; }
        public static String Oid_modelo { get; set; }
        public static String Oid_mac { get; set; }
        public static String Oid_tipo_impressora { get; set; }
        public static String Oid_serial { get; set; }
        public static String Id_cliente { get; set; }
    }
}
