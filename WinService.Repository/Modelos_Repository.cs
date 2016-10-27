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
    public class Modelos_Repository : Conexao_Mysql
    {

        public ModeloDTO Pesquisa_Modelo(String Nome_Modelo)
        {

            ModeloDTO lista_pesq = new ModeloDTO();
            try
            {
                AbrirConexao();
                Cmd = new MySqlCommand("Select ID,Nome From tarifador.modelos where Nome=@v1", Con);
                Cmd.Parameters.AddWithValue("@v1", Nome_Modelo);

                Dr = Cmd.ExecuteReader();


                var resultado = Dr.HasRows;
                if (resultado == true)
                {
                    if (Dr.Read())

                        lista_pesq.Id_Modelo = Convert.ToString(Dr["ID"]);
                    lista_pesq.Nome_Modelo = Convert.ToString(Dr["Nome"]);


                }


            }
            catch (Exception)
            {

                lista_pesq.Id_Modelo = "0";
                lista_pesq.Nome_Modelo = "Erro";

            }
            finally
            {
                FecharConexao();
            }
            return lista_pesq;

        }


        public String Cria_modelo(String Nome, String Id_fabricante)
        {
            String Retorno="";
            try
            {
                AbrirConexao();
                Cmd = new MySqlCommand();
                Cmd.Connection = Con;

                String SrtSql = "INSERT INTO `tarifador`.`modelos`";

                SrtSql += " (`id`,";
                SrtSql += "`nome`,";
                SrtSql += "`id_fabricante`)";
                SrtSql += "VALUES ";

                SrtSql += " (uuid(),";
                SrtSql += " @v2,";
                SrtSql += "@v3)";

                Cmd.CommandText = SrtSql;


                Cmd.Parameters.AddWithValue("@v2", Nome);
                Cmd.Parameters.AddWithValue("@v3", Id_fabricante);

                Cmd.ExecuteNonQuery();

                String str2 = "SELECT `id` FROM `tarifador`.`modelos` AS last_id WHERE `nome`='"+ Nome+"'";
                Cmd = new MySqlCommand(str2, Con);
                Dr = Cmd.ExecuteReader();

                var resultado = Dr.HasRows;

                if (resultado == true)
                {
                    //Se Ok retorna o numero do registro criado
                    while (Dr.Read())
                    {
                        Retorno = Convert.ToString(Dr["id"]);

                    }
                    Dr.Close();
                }






            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Inserir Modelo " + ex.Message);
               
            }
            finally
            {
                FecharConexao();
            }

            return Retorno;
        }

        public String Cria_Modelos_oids(String Id_Modelo, String Id_Oid)
        {
            String Retorno = "";
            try
            {
                AbrirConexao();
                Cmd = new MySqlCommand();
                Cmd.Connection = Con;

                String SrtSql = "INSERT INTO `tarifador`.`modelos_oids`";
               
                SrtSql += "(`id_modelo`,";
                SrtSql += "`id_oid`)";
                SrtSql += "VALUES (";
                SrtSql += " @v2,";
                SrtSql += "@v3)";

                Cmd.CommandText = SrtSql;


                Cmd.Parameters.AddWithValue("@v2", Id_Modelo);
                Cmd.Parameters.AddWithValue("@v3", Id_Oid);

                Cmd.ExecuteNonQuery();
                               
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Inserir Modelos_oids " + ex.Message);

            }
            finally
            {
                FecharConexao();
            }

            return Retorno;
        }
    }
}
