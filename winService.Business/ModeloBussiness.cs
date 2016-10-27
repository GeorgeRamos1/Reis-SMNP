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
    public class ModeloBussiness
    {
        Modelos_Repository _ModeloRepository = new Modelos_Repository();

        public ModeloDTO PesquisaModelo(String Nome)
        {


            var Modelo_Pesquisado = _ModeloRepository.Pesquisa_Modelo(Nome);

            return Modelo_Pesquisado;

        }

    }
}
