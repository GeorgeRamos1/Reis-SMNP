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


            String Str = "SELECT A.numero_serie, E.OID ";
            Str += " FROM tarifador.equipamentos A ,";
            Str += " modelos B,";
            Str += " modelos_oids_grupos C,";
            Str += " oids_grupos_oids D,";
            Str += " oids E";
            Str += " WHERE A.id_modelo=B.id AND ";
            Str += " A.id_modelo = C.id_modelo AND ";
            Str += " C.ID_GRUPO = D.ID_GRUPO AND ";
            Str += " D.ID_OID=E.ID AND";
            Str += " A.numero_serie=@v1";


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







    }
}
