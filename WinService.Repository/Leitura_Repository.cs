using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DAL;
using WinService.DTO;
using System.Data.SQLite;
using WinService.Utilities;
using System.Data;

using System.Diagnostics;


namespace WinService.Repository
{
    public class Leitura_Repository : Conexao_Sqlite
    {
        LogEvento _log = new LogEvento();

        public DateTime? Insere_Leitura(OID_Leitura Leitura)
        {


             
            DateTime? data_leitura = DateTime.Now;
            var resposta = data_leitura;
            try
            {

                AbrirConexao();
                using (Cmd = new SQLiteCommand())
                {
                    
                    Cmd.Connection = Con;


                    String SrtSql = "INSERT INTO leitura_log";

                    SrtSql += " (id,";
                    SrtSql += "Id_equipamento,";
                    SrtSql += "Id_Oid,";
                    SrtSql += "valor,";
                    SrtSql += "data_leitura)";
                    SrtSql += "VALUES ";

                    SrtSql += " ('" + Guid.NewGuid() + "',";
                    SrtSql += " @v2,";
                    SrtSql += " (SELECT id  FROM oids WHERE oid =  @v3),";// função do DB para capturar o Id do OID
                    SrtSql += " @v4,";
                    SrtSql += "@v5)";

                    Cmd.CommandText = SrtSql;


                    Cmd.Parameters.AddWithValue("@v2", Leitura.Id_equipamento);
                    Cmd.Parameters.AddWithValue("@v3", Leitura.OID);
                    Cmd.Parameters.AddWithValue("@v4", Leitura.Valor);
                    Cmd.Parameters.AddWithValue("@v5", data_leitura);

                    Cmd.ExecuteNonQuery();
                    
                }

                FecharConexao();

            }
            catch (Exception ex)
            {
                _log.WriteEntry("Erro ao Inserir Leitura de OID :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
                //throw new Exception("Erro ao Inserir Modelo " + ex.Message);
                resposta = null;
            }
            finally
            {
                FecharConexao();
            }

            return resposta;
        }


        public DateTime? InserirListaLogOID(IEnumerable<OID_Leitura> lista)
        {
            DateTime? data_leitura = DateTime.Now;
            var resposta = data_leitura;

            var listaCovertida = new List<OID_Leitura>();

            foreach (var item in lista)
            {
                OID_Leitura listaCapturada = new OID_Leitura();

                listaCapturada.Dt_leitura = item.Dt_leitura;
                listaCapturada.Dt_Transmissao = item.Dt_Transmissao;
                listaCapturada.Id_Cliente = item.Id_Cliente;
                listaCapturada.Id_equipamento = item.Id_equipamento;
                listaCapturada.Id_Equipamento = item.Id_Equipamento;
                listaCapturada.IP = item.IP;
                listaCapturada.Mac_address = item.Mac_address;
                listaCapturada.maq_contrato = item.maq_contrato;
                listaCapturada.Nr_Serie = item.Nr_Serie;
                listaCapturada.OID = item.OID;
                listaCapturada.Valor = item.Valor;
                listaCovertida.Add(listaCapturada);
            }


            //Trasforma a classe LeituraLogDTO em um DataTable para inserir em um só bloco
            DataTable dt = Converts.ToDataTable<OID_Leitura>(listaCovertida);

            int NaoInseriuRegistros = 0;
            int RegistrosInseridos = 0;

            // string commandText = "insert into tarifador_srv.leitura_log (id,id_equipamento,id_oid,valor,data_leitura) ";
            // commandText += " values (@id,@id_equipamento,@id_oid,@valor,@data_leitura)";


            String SrtSql = "INSERT INTO `leitura_log`";

            SrtSql += " (`id`,";
            SrtSql += "`Id_equipamento`,";
            SrtSql += "`Id_Oid`,";
            SrtSql += "`valor`,";
            SrtSql += "`data_leitura`)";
            SrtSql += "VALUES ";

            SrtSql += " (uuid(),";
            SrtSql += " @id_equipamento,";
            SrtSql += "CapturaID_Oid(@oid),";// função do DB para capturar o Id do OID
            SrtSql += " @valor,";
            SrtSql += "@data_leitura)";

            AbrirConexao();

            try
            {


                using (SQLiteCommand cmd = new SQLiteCommand(SrtSql, Con))
                {
                    cmd.UpdatedRowSource = UpdateRowSource.None;


                    cmd.Parameters.Add("@id_equipamento",DbType.String).SourceColumn = "Id_equipamento";
                    cmd.Parameters.Add("@oid", DbType.String).SourceColumn = "oid";
                    cmd.Parameters.Add("@valor", DbType.String).SourceColumn = "valor";
                    cmd.Parameters.Add("@data_leitura",DbType.String).SourceColumn="Dt_leitura";


                    SQLiteDataAdapter da = new SQLiteDataAdapter();
                    da.InsertCommand = cmd;


                    RegistrosInseridos = da.Update(dt);

                    dt.Dispose();
                    FecharConexao();

                }

            }
            catch (Exception ex)
            {
                _log.WriteEntry("Erro ao Inserir Leitura_log: " + ex.Message.ToString(), EventLogEntryType.Error);
                return resposta;
            }
            finally
            {
                FecharConexao();

            }


            return resposta;

        }

        public DateTime? Insere_LeituraLista(IEnumerable<OID_Leitura> lista)
        {



            DateTime? data_leitura = DateTime.Now;
            var resposta = data_leitura;
            try
            {

                foreach (var item in lista)
                {
                 resposta =   this.Insere_Leitura(item);


                }


            }
            catch (Exception ex)
            {
                _log.WriteEntry("Erro ao Inserir Leitura de OID :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
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
