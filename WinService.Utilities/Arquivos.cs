using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WinService.Utilities
{
    public class Arquivos
    {
        public void EscreveArquivo(String Arq,String body)


        {
            //Declaração do método StreamWriter passando o caminho e nome do arquivo que deve ser salvo
            StreamWriter writer = new StreamWriter(Arq);
            //Escrevendo o Arquivo e pulando uma linha
            writer.WriteLine(body);
           
            //Fechando o arquivo
            writer.Close();
            //Limpando a referencia dele da memória
            writer.Dispose();
        }

        public String LeArquivo(String Arq)
        {
            // cria um leitor e abre o arquivo
            StreamReader Read = new StreamReader(Arq);

            String Body = Read.ReadLine();

            return Body;



        }




    }
}
