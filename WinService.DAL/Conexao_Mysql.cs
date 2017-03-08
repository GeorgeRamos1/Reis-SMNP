using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using WinService.Utilities;

namespace WinService.DAL
{
    public class Conexao_Mysql
    {
        LogEvento _log = new LogEvento();

        protected MySqlConnection Con; // faz a conexao
        protected MySqlCommand Cmd; // Escrever ou executar comanados
        protected MySqlDataReader Dr; //guarda regustros obtidos de querys

        protected MySqlDataAdapter Da;
        //abrir conexao

        protected void AbrirConexao()
        {
            var Conn_str = ConfigurationManager.AppSettings["Conn_db"];
            try
            {
                //Local
                  string path = "VM-WinServer";

                //Servidor
              //  string path = "192.168.2.107";

                //String de conexão
                  Con = new MySqlConnection(Conn_str);
                Con.Open();

            }
            catch (Exception ex)
            {

             //   throw new Exception(ex.Message);
                _log.WriteEntry("Erro ao abrir conexao com o db :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);

            }
        }


        protected void FecharConexao()
        {
            try
            {
                Con.Close();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
