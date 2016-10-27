using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.DAL;
using MySql.Data.MySqlClient;
using System.Data;

namespace WinService.Repository
{
    public class Equipamentos_Repository : Conexao_Mysql
    {




        public IEnumerable<OIDs_Dispositivo> getOIDsDispositivo(String vIp, String vNr_Serie)
        {

            // O union é porque para alguns caso o grupo pode não ser criado
            String Str = "SELECT A.id,A.numero_serie, E.OID";
            Str += " FROM tarifador.equipamentos A left join";
            Str += " modelos B on (A.id_modelo=B.id) left join ";
            Str += " modelos_oids_grupos C on( A.id_modelo = C.id_modelo) Left join";
            Str += " oids_grupos_oids D on ( C.ID_GRUPO = D.ID_GRUPO) left join";
            Str += " oids E on(D.ID_OID=E.ID)";
            Str += " WHERE  A.numero_serie=@v1  ";
            Str += " union  ";
            Str += " SELECT A.id,";
            Str += " A.numero_serie,";
            Str += " D.OID ";
            Str += " FROM tarifador.equipamentos A left Join ";
            Str += " tarifador.modelos B on (A.id_modelo = B.id) left join";
            Str += " tarifador.modelos_oids C on ( B.id =C.id_modelo) left join";
            Str += " oids D  on (C.Id_Oid =D.id)";
            Str += " WHERE  A.numero_serie=@v1";


            var lista_OID_Capturado = new List<OIDs_Dispositivo>();

            try
            {
                AbrirConexao();
                Cmd = new MySqlCommand(Str, Con);
                Cmd.Parameters.AddWithValue("@v1", vNr_Serie);

                Dr = Cmd.ExecuteReader();


                while (Dr.Read())
                {
                    var lista_Oid = new OIDs_Dispositivo();



                    lista_Oid.IP = vIp;
                    lista_Oid.OID = Dr["oid"].ToString();
                    lista_Oid.Nr_Serie = vNr_Serie;
                    lista_Oid.maq_contrato = "SIM";
                    lista_Oid.Id_equipamento = Dr["Id"].ToString();

                    //Adiciona na lista

                    lista_OID_Capturado.Add(lista_Oid);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Listar OIDs do Dispositivo: " + ex.Message);
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
                Cmd = new MySqlCommand();
                Cmd.Connection = Con;

                String SrtSql = "INSERT INTO `tarifador`.`equipamentos`";

                SrtSql += " (`id`,";
                SrtSql += "`Id_cliente`,";
                SrtSql += "`Id_Modelo`,";
                SrtSql += "`numero_serie`,";
                SrtSql += "`ip_atual`,";
                SrtSql += "`contrato`)";
                SrtSql += "VALUES ";

                SrtSql += " (uuid(),";
                SrtSql += " @v2,";
                SrtSql += " @v3,";
                SrtSql += " @v4,";
                SrtSql += " @v5,";
                SrtSql += " @v6)";

                Cmd.CommandText = SrtSql;

                Cmd.Parameters.AddWithValue("@v2", vDadosDispositivo.Id_cliente);
                Cmd.Parameters.AddWithValue("@v3", vDadosDispositivo.Modelo);
                Cmd.Parameters.AddWithValue("@v4", vDadosDispositivo.Nr_Serie);
                Cmd.Parameters.AddWithValue("@v5", vDadosDispositivo.IP);
                Cmd.Parameters.AddWithValue("@v6", vDadosDispositivo.maq_contrato);

                Cmd.ExecuteNonQuery();

                String str2 = "SELECT `id` FROM `tarifador`.`equipamentos` WHERE `numero_serie`='" + vDadosDispositivo.Nr_Serie + "'";
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




    }
}
