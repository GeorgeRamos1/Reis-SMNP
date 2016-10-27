using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WinService.Business;
using WinService.DTO;
namespace WinService.UnitTest
{
    public class ModeloTest
    {
        [TestFixture]
        public sealed class LeituraTest
        {
            ModeloBussiness _BLLModelo = new ModeloBussiness();

            [Test]
            public ModeloDTO Testar_Captura_De_OID_Dispositivo()
            {
                var resultado = _BLLModelo.PesquisaModelo("0");

                return resultado;


            }


        }
    }
}
