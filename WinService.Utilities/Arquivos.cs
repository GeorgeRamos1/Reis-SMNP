using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace WinService.Utilities
{
    public class Arquivos
    {
        LogEvento _log = new LogEvento();
        public void EscreveArquivo(String Arq,String body)


        {

            try
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
            catch (Exception ex)
            {

                _log.WriteEntry("Erro ao Escrever arquivo: " + ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }

        public String LeArquivo(String Arq)
        {

            String Body = "";


            try
            {
                // cria um leitor e abre o arquivo
                StreamReader Read = new StreamReader(Arq);

               Body = Read.ReadLine();

                
            }
            catch (Exception ex)
            {

                _log.WriteEntry("Erro ao ler Arquivo: " + ex.ToString() , System.Diagnostics.EventLogEntryType.Error);
            }


            return Body;
        }


        public String CaminhoAppData()
        {
            String caminho = "C:\\RO\\LeituraOID";

       
         
           // return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return caminho;
           
        }

        public String BackupDb()
        {
            String resposta = "OK";
            try
            {
                String caminho = this.CaminhoAppData();
                String targetPath = caminho + @"\tarifador.bk";

                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                }
               

                //copia para o local com o nome alterado
                System.IO.File.Copy(caminho + @"\tarifador.db", targetPath);
            }
            catch (Exception ex)
            {
                _log.WriteEntry("Backup ERRO :"+ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
                resposta = "ERRO";
            }

            return resposta;
        }
    }



}
