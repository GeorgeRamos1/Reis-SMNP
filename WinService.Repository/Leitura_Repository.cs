using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DAL;
using WinService.DTO;
using MySql.Data.MySqlClient;



namespace WinService.Repository
{
    public class Leitura_Repository : Conexao_Mysql
    {
        

        public DateTime? Insere_Leitura(OID_Leitura Leitura)
        {



            DateTime? data_leitura = DateTime.Now;
            var resposta = data_leitura;
            try
            {
               
                AbrirConexao();
                Cmd = new MySqlCommand();
                Cmd.Connection = Con;

                String SrtSql = "INSERT INTO `tarifador`.`leitura_log`";

                SrtSql += " (`id`,";
                SrtSql += "`Id_equipamento`,";
                SrtSql += "`Id_Oid`,";
                SrtSql += "`valor`,";
                SrtSql += "`data_leitura`)";
                SrtSql += "VALUES ";

                SrtSql += " (uuid(),";
                SrtSql += " @v2,";
                SrtSql += "CapturaID_Oid( @v3),";// função do DB para capturar o Id do OID
                SrtSql += " @v4,";
                SrtSql += "@v5)";

                Cmd.CommandText = SrtSql;


                Cmd.Parameters.AddWithValue("@v2", Leitura.Id_equipamento);
                Cmd.Parameters.AddWithValue("@v3", Leitura.OID);
                Cmd.Parameters.AddWithValue("@v4", Leitura.Valor);
                Cmd.Parameters.AddWithValue("@v5", data_leitura);

                Cmd.ExecuteNonQuery();
             

            }
            catch (Exception )
            {

                //throw new Exception("Erro ao Inserir Modelo " + ex.Message);
                resposta = null;
            }
            finally
            {
                FecharConexao();
            }

            return resposta;
        }



     


    }
}
