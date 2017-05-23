using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using WinService.Utilities;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using WinService.DTO;

namespace WinService.DAL
{
    public class Conexao_Sqlite
    {
 //Cria um log de Eventos
        LogEvento appLog = new LogEvento();


        protected SQLiteConnection Con;
        protected SQLiteCommand Cmd;
        protected SQLiteDataReader Dr;
        protected SQLiteDataAdapter Da;

        protected void AbrirConexao()
        {

            try
            {
               
                String Conn_str;
                //String de conexão


                //Servidor - captura do arquivo de configuração
                //Conn_str =  ConfigurationManager.AppSettings["Conn_db_SQLite"];

                Conn_str = "Data Source=" + Global.Path_Cert + "\\tarifador.db;Version=3;";
                    
                Con = new SQLiteConnection(Conn_str);
                Con.Open();

            }
            catch (Exception ex)
            {
                appLog.WriteEntry("Erro de Conexao do DB: " + ex.Message, EventLogEntryType.Error);
            }
        }
        protected void FecharConexao()
        {
            try
            {


                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                    //  Con.Dispose();
                }
            }
            catch (Exception ex)
            {

                appLog.WriteEntry("Erro de Fechar conexao do DB: " + ex.Message, EventLogEntryType.Error);
            }

        }

    }
}
