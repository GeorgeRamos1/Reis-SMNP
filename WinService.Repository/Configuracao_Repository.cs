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
    public class Configuracao_Repository  : Conexao_Mysql
    {

        public Int32 AtualizaDataLeituraEmConfiguracaoCliente(String Id ,DateTime? dataLeitura, DateTime? dataTransmissao)
        {

           
            Int32 retorno = WinService.DTO.Contantes.SUCESSO;

            // Para executar é necessário que exista um id e uma das duas datas
            if (String.IsNullOrEmpty(Id.ToString()) != true && String.IsNullOrEmpty(dataLeitura.ToString()) != true ||
                String.IsNullOrEmpty(Id.ToString()) != true && String.IsNullOrEmpty(dataTransmissao.ToString()) != true)
            {

                try
                {
                    AbrirConexao();

                    Cmd = new MySqlCommand();
                    Cmd.Connection = Con;


                    String Sql = "Update `tarifador`.`clientes` SET ";

                    if (String.IsNullOrEmpty(dataLeitura.ToString()) != true)
                    {
                        Sql += " `data_ultima_leitura`=@V1"; 
                    }

                    if (String.IsNullOrEmpty(dataTransmissao.ToString()) != true && String.IsNullOrEmpty(dataLeitura.ToString()) != true)
                    {
                        Sql += ","; 
                    }
                    if (String.IsNullOrEmpty(dataTransmissao.ToString()) != true)
                    {
                        Sql += " `data_transmissao`=@V2";
                    }
                    Sql += " where id =@V3";

                    Cmd.CommandText = Sql;
                    Cmd.Parameters.AddWithValue("@V1", dataLeitura);
                    Cmd.Parameters.AddWithValue("@V2", dataTransmissao);
                    Cmd.Parameters.AddWithValue("@V3", Id);
                


                    Cmd.ExecuteNonQuery();



                    retorno = Convert.ToInt32(Cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    retorno = WinService.DTO.Contantes.ERRO_AO_EDITAR;
                    throw new Exception("Erro ao editar cliente " + ex.Message);
                }
                finally
                {

                    FecharConexao();
                }
            }

            return retorno;


        }

    }
}
