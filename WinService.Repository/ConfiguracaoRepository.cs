using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.DAL;
using System.Data.SQLite;
using System.Data;
using WinService.Utilities;
using System.Diagnostics;


namespace WinService.Repository
{
    public class ConfiguracaoRepository : Conexao_Sqlite
    {


        //Cria um log de Eventos
        LogEvento appLog = new LogEvento();

        public ConfiguracaoDTO ListarConfiguracao(Int32 Id)
        {
            ConfiguracaoDTO DadosConfiguracao = new ConfiguracaoDTO();
            try
            {
                AbrirConexao();

                String str = "Select id,";
                str += "id_cliente,";
                str += "ciclo_leitura,";
                str += "ciclo_upload,";
                str += "ip_pc,";
                str += "range_ip,";
                str += "url_ws,";
                str += "nr_contrato";
                str += " From configuracao where id=@v1";

                using (Cmd = new SQLiteCommand(str, Con))
                {


                    Cmd.Parameters.AddWithValue("@v1", Id);

                    Dr = Cmd.ExecuteReader();


                    var resultado = Dr.HasRows;
                    if (resultado == true)
                    {
                        if (Dr.Read())

                            DadosConfiguracao.Id = Convert.ToInt32(Dr["id"]);
                        DadosConfiguracao.Id_Cliente = Convert.ToString(Dr["id_cliente"]);
                        DadosConfiguracao.Ciclo_Leitura = Convert.ToInt32(Dr["ciclo_leitura"]);
                        DadosConfiguracao.Ciclo_UpLoad = Convert.ToInt32(Dr["ciclo_upload"]);
                        DadosConfiguracao.Ip_Pc = Convert.ToString(Dr["ip_pc"]);
                        DadosConfiguracao.Range_Ip = Convert.ToString(Dr["range_ip"]);
                        DadosConfiguracao.Url_Ws = Convert.ToString(Dr["url_ws"]);
                        DadosConfiguracao.Nr_Contrato = Convert.ToString(Dr["nr_contrato"]);


                        Dr.Dispose();
                        FecharConexao();
                    }

                }
            }
            catch (Exception ex)
            {
                appLog.WriteEntry("Erro ao Ler dados do Cliente: " + ex.Message, EventLogEntryType.Error);
                DadosConfiguracao.Id = 0;
                DadosConfiguracao.Id_Cliente = "Erro";

            }
            finally
            {
                FecharConexao();
            }
            return DadosConfiguracao;
        }


        public string EditarRegistro(ConfiguracaoDTO item)
        {

            String retorno = "OK";

            try
            {
                AbrirConexao();

                Cmd = new SQLiteCommand();
                Cmd.Connection = Con;

                String Sql = "Update `configuracao` SET ";
                Sql += "id_cliente=@v2,";
                Sql += "ciclo_leitura=@v3,";
                Sql += "ciclo_upload=@v4,";
                Sql += "ip_pc=@v5,";
                Sql += "range_ip=@v6,";
                Sql += "url_ws=@v7,";
                Sql += "nr_contrato=@v8";

                Sql += "  where `id`=@v1";


                Cmd.CommandText = Sql;
                Cmd.Parameters.AddWithValue("@v1", item.Id);
                Cmd.Parameters.AddWithValue("@v2", item.Id_Cliente);
                Cmd.Parameters.AddWithValue("@v3", item.Ciclo_Leitura);
                Cmd.Parameters.AddWithValue("@v4", item.Ciclo_UpLoad);
                Cmd.Parameters.AddWithValue("@v5", item.Ip_Pc);
                Cmd.Parameters.AddWithValue("@v6", item.Range_Ip);
                Cmd.Parameters.AddWithValue("@v7", item.Url_Ws);
                Cmd.Parameters.AddWithValue("@v8", item.Nr_Contrato);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                retorno = "Erro";
                appLog.WriteEntry("Erro ao atualizar configuração: " + ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                FecharConexao();
            }

            return retorno;
        }

        public string InsereRegistro(ConfiguracaoDTO item)
        {
            String retorno = "OK";
            try
            {
                AbrirConexao();
                using (Cmd = new SQLiteCommand())
                {

                    Cmd.Connection = Con;

                    String SrtSql = "INSERT INTO configuracao";

                    SrtSql += " (id,";
                    SrtSql += "id_cliente,";
                    SrtSql += "ciclo_leitura,";
                    SrtSql += "ciclo_upload,";
                    SrtSql += "ip_pc,";
                    SrtSql += "range_ip,";
                    SrtSql += "url_ws,";
                    SrtSql += "nr_contrato)";

                    SrtSql += "VALUES ";
                    SrtSql += " (@v1,";
                    SrtSql += " @v2,";
                    SrtSql += " @v3,";
                    SrtSql += " @v4,";
                    SrtSql += " @v5,";
                    SrtSql += " @v6,";
                    SrtSql += " @v7,";
                    SrtSql += " @v8)";

                    Cmd.CommandText = SrtSql;

                    Cmd.Parameters.AddWithValue("@v1", item.Id);
                    Cmd.Parameters.AddWithValue("@v2", item.Id_Cliente);
                    Cmd.Parameters.AddWithValue("@v3", item.Ciclo_Leitura);
                    Cmd.Parameters.AddWithValue("@v4", item.Ciclo_UpLoad);
                    Cmd.Parameters.AddWithValue("@v5", item.Ip_Pc);
                    Cmd.Parameters.AddWithValue("@v6", item.Range_Ip);
                    Cmd.Parameters.AddWithValue("@v7", item.Url_Ws);

                    Cmd.Parameters.AddWithValue("@v8", item.Nr_Contrato);

                    Cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                appLog.WriteEntry("Erro ao Inserir configuração :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
                retorno = "Erro";
            }
            finally
            {
                FecharConexao();
            }

            return retorno;


        }



    }
}
