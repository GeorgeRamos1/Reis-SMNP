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


        public Int32 Insere_Leitura(OID_Leitura Leitura)
        {
int resposta =0;
String Retorno = "";
String cliente_desconhecido = "8609F87D-4BC4-4D70-8792-EE429F160A61";
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
                SrtSql += " @v3,";
                SrtSql += " @v4,";
                SrtSql += "now())";

                Cmd.CommandText = SrtSql;


                Cmd.Parameters.AddWithValue("@v2", Leitura.Id_equipamento);
                Cmd.Parameters.AddWithValue("@v3", Leitura.OID);
                Cmd.Parameters.AddWithValue("@v4", Leitura.Valor);
                Cmd.ExecuteNonQuery();
             

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Inserir Modelo " + ex.Message);

            }
            finally
            {
                FecharConexao();
            }

            return resposta;
        }






    }
}
