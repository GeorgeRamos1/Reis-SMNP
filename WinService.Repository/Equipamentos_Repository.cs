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

namespace WinService.Repository
{
    public class Equipamentos_Repository : Conexao_Sqlite
    {

        LogEvento _log = new LogEvento();


        public IEnumerable<OIDs_Dispositivo> getOIDsDispositivo(String vIp, String vMacAddress)
        {
   

            String Str = "SELECT Id,numero_serie,OID  FROM listar_dispositivos";
            Str += " WHERE  mac_address=@v1";
         
            var lista_OID_Capturado = new List<OIDs_Dispositivo>();

            try
            {
                AbrirConexao();
                using (Cmd = new SQLiteCommand(Str, Con))
                {
                    
                    Cmd.Parameters.AddWithValue("@v1", vMacAddress);

                    Dr = Cmd.ExecuteReader();


                    while (Dr.Read())
                    {
                        var lista_Oid = new OIDs_Dispositivo();

                        lista_Oid.IP = vIp;
                        lista_Oid.OID = Dr["oid"].ToString();
                        lista_Oid.Nr_Serie = Dr["numero_serie"].ToString();
                        lista_Oid.Mac_address = vMacAddress;
                        lista_Oid.Id_equipamento = Dr["Id"].ToString();

                        //Adiciona na lista

                        lista_OID_Capturado.Add(lista_Oid);
                    }
                    Dr.Dispose();
                    FecharConexao();
                }

            }
            catch (Exception ex)
            {
                //throw new Exception("Erro ao Listar OIDs do Dispositivo: " + ex.Message);
                _log.WriteEntry("Erro em getOIDsDispositivo :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);

            }
            finally
            {
               
                FecharConexao();
            }

            return lista_OID_Capturado;

        }


        public String Cria_Dispositivo(EquipamentosDTO vDadosDispositivo)
        {
            String Retorno = "";
            try
            {
                AbrirConexao();
                using (Cmd = new SQLiteCommand())
                {
                    
                    Cmd.Connection = Con;

                    String SrtSql = "INSERT INTO equipamentos";

                    SrtSql += " (id,";
                    SrtSql += "Id_cliente,";
                    SrtSql += "Id_Modelo,";
                    SrtSql += "numero_serie,";
                    SrtSql += "ip_atual,";
                    SrtSql += "contrato,";
                    SrtSql += "mac_address)";
                    SrtSql += "VALUES ";

                    SrtSql += " ('" + Guid.NewGuid() + "',";
                    SrtSql += " @v2,";
                    SrtSql += " @v3,";
                    SrtSql += " @v4,";
                    SrtSql += " @v5,";
                    SrtSql += " @v6,";
                    SrtSql += " @v7)";

                    Cmd.CommandText = SrtSql;

                    Cmd.Parameters.AddWithValue("@v2", vDadosDispositivo.Id_cliente);
                    Cmd.Parameters.AddWithValue("@v3", vDadosDispositivo.Modelo);
                    Cmd.Parameters.AddWithValue("@v4", vDadosDispositivo.Nr_Serie);
                    Cmd.Parameters.AddWithValue("@v5", vDadosDispositivo.IP);
                    Cmd.Parameters.AddWithValue("@v6", vDadosDispositivo.maq_contrato);
                    Cmd.Parameters.AddWithValue("@v7", vDadosDispositivo.Mac_address);

                    Cmd.ExecuteNonQuery();

                    String str2 = "SELECT id FROM equipamentos WHERE numero_serie='" + vDadosDispositivo.Nr_Serie + "'";
                    Cmd = new SQLiteCommand(str2, Con);
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

                    Dr.Dispose();
                    FecharConexao();
                }
            }
            catch (Exception ex)
            {

                // throw new Exception("Erro ao Inserir Modelo " + ex.Message);
                _log.WriteEntry("Erro ao Inserir Modelo :" + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                FecharConexao();
            }

            return Retorno;
        }




    }
}
