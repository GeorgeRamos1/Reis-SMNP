using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DAL;
using WinService.DTO;
using System.Data.SQLite;
using WinService.Utilities;

namespace WinService.Repository
{
    public class Configuracao_Repository  : Conexao_Sqlite
    {

        public Int32 AtualizaDataLeituraEmConfiguracaoCliente(String Id ,DateTime? dataLeitura, DateTime? dataTransmissao)
        {

            LogEvento _log = new LogEvento();
            Int32 retorno = WinService.DTO.Contantes.SUCESSO;

            // Para executar é necessário que exista um id e uma das duas datas
            if (String.IsNullOrEmpty(Id.ToString()) != true && String.IsNullOrEmpty(dataLeitura.ToString()) != true ||
                String.IsNullOrEmpty(Id.ToString()) != true && String.IsNullOrEmpty(dataTransmissao.ToString()) != true)
            {

                try
                {
                    AbrirConexao();

                    using (Cmd = new SQLiteCommand())
                    {
                        
                        Cmd.Connection = Con;


                        String Sql = "Update clientes SET ";

                        if (String.IsNullOrEmpty(dataLeitura.ToString()) != true)
                        {
                            Sql += " data_ultima_leitura=@V1";
                        }

                        if (String.IsNullOrEmpty(dataTransmissao.ToString()) != true && String.IsNullOrEmpty(dataLeitura.ToString()) != true)
                        {
                            Sql += ",";
                        }
                        if (String.IsNullOrEmpty(dataTransmissao.ToString()) != true)
                        {
                            Sql += " data_transmissao=@V2";
                        }
                        Sql += " where id =@V3";

                        Cmd.CommandText = Sql;
                        Cmd.Parameters.AddWithValue("@V1", dataLeitura.ToString());
                        Cmd.Parameters.AddWithValue("@V2", dataTransmissao.ToString());
                        Cmd.Parameters.AddWithValue("@V3", Id);



                        Cmd.ExecuteNonQuery();
                        retorno = Convert.ToInt32(Cmd.ExecuteScalar());




                    }



                   
                    FecharConexao();
                }
                catch (Exception ex)
                {
                    retorno = WinService.DTO.Contantes.ERRO_AO_EDITAR;
                   // throw new Exception("Erro ao editar cliente " + ex.Message);

                    _log.WriteEntry("Erro ao editar cliente :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
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
