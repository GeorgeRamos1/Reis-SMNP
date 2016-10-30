using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.Repository;
using WinService.Utilities;

namespace WinService.Business
{
    public class EquipamentosBusiness
    {

        Equipamentos_Repository _repositoryDispositivo = new Equipamentos_Repository();
        Leitura_Repository _repositoryLeitura = new Leitura_Repository();
        Modelos_Repository _ModeloRepository = new Modelos_Repository();
        Snmp _SNMP = new Snmp();

        //Busca os Ip que estão respondendo na rede como impressora
        public IEnumerable<EquipamentosDTO> Captura_Ip_Dipositivos_Na_rede()
        {
            //Captura o range de IP
            String vRange = "10.0.0.";
            String vOid_Serial = "1.3.6.1.2.1.43.5.1.1.17.1";
            String vOid_Mac = "1.3.6.1.2.1.2.2.1.6.1";
            //  String vOid_Model = "1.3.6.1.2.1.43.5.1.1.16.1";
            String vOid_Model = "1.3.6.1.2.1.25.3.2.1.3.1";
            String vOid_prn = "1.3.6.1.2.1.43.5.1.1.1.1";
            String vIp;
            var lista_Dispostivos_encontrados = new List<EquipamentosDTO>();

            var dt_inicio = DateTime.Now;
            for (int i = 30; i <255; i++)
            {

                vIp = vRange + i.ToString();

                var result = _SNMP.capturaOID(vIp, vOid_prn);

                if (String.IsNullOrEmpty(result.Id) == false)
                {
                    var lista_pesq = new EquipamentosDTO();

                    var vNrSerie = _SNMP.capturaOID(vIp, vOid_Serial);
                    // var vMac = _SNMP.capturaOID(vIp, vOid_Mac);
                    var VModel = _SNMP.capturaOID(vIp, vOid_Model);

                    lista_pesq.IP = vIp;
                    lista_pesq.Nr_Serie = vNrSerie.Valor;
                    //    lista_pesq.MAcAdress = vMac.Valor;
                    lista_pesq.Modelo = VModel.Valor;


                    lista_Dispostivos_encontrados.Add(lista_pesq);
                }

            }
            var dt_Fim = DateTime.Now;

            var Tempo_loop = dt_Fim - dt_inicio;

            return lista_Dispostivos_encontrados;
        }

        // Para a lista de dispositivos encontrados buscar o grupo de OIDs Correspondentes para leitura
        public IEnumerable<OIDs_Dispositivo> Captura_OID_Dispositivo(IEnumerable<EquipamentosDTO> Lista_Dispositivo)
        {

          //  var lista_OID_Todos_Dispositivos = new List<OIDs_Dispositivo>();
            var lista_OID_Lidos = new List<OIDs_Dispositivo>();
            foreach (var Dispositivo in Lista_Dispositivo)
            {

                

                // PEsquisa se o Dispositivo Existe na lista de Equipamentos/Matriz/Modelo e se pertence a Empresa
                //Se não existir utiliza-se da matriz padrão

                var Dispositivos_Encontrados = _repositoryDispositivo.getOIDsDispositivo(Dispositivo.IP, Dispositivo.Nr_Serie);

                //Caso não seja encontrado nenhum registro, criar com  OID padrão para leitura.
                //Salva registro no cadastro de equipamentos
                if (Dispositivos_Encontrados.Count() == 0)
                {
                    //Insere o equipamento na lista para informação, porém não faz parte do contrato
                    var Id_Dispositivo_Retornado = Insere_Equipamento(Dispositivo.IP, Dispositivo.Modelo, Dispositivo.Nr_Serie, "NAO");


                    lista_OID_Lidos = new List<OIDs_Dispositivo> { new OIDs_Dispositivo { IP = Dispositivo.IP, Nr_Serie = Dispositivo.Nr_Serie, OID = "1.3.6.1.2.1.43.10.2.1.4.1.1", maq_contrato = "NAO", Id_equipamento = Id_Dispositivo_Retornado } };


                }
                else
                {

                    foreach (var item_dispositivo in Dispositivos_Encontrados)
                    {
                        var item_capturado = new OIDs_Dispositivo();
                        // se não encontrar o OID de contador na tabela, usa o oid de contador geral
                       
                        item_capturado.IP = item_dispositivo.IP;
                        item_capturado.Nr_Serie = item_dispositivo.Nr_Serie;
                        if (String.IsNullOrEmpty(item_dispositivo.OID) == true)
                        { item_capturado.OID = "1.3.6.1.2.1.43.10.2.1.4.1.1"; }
                        else
                        {
                            item_capturado.OID = item_dispositivo.OID;
                        }

                        item_capturado.Id_equipamento = item_dispositivo.Id_equipamento;

                        lista_OID_Lidos.Add(item_capturado);
                    }

                    

                }


               // lista_OID_Todos_Dispositivos.AddRange(lista_OID_Lidos);
            }


            return lista_OID_Lidos;

        }

        //Insere no DB os equipamentos lidos na rede mas não constam no DB
        public String Insere_Equipamento(String IP, String Modelo, String NS, String contrato)
        {
            //busca o ID do cliente instalado 
            String vId_Cliente = "6aa3d5f0-9056-11e6-bc34-00155d019604";
            String vId_fabricante = "00000000-0000-0000-00000-000000000000";
            var pesquisa_modelo = _ModeloRepository.Pesquisa_Modelo(Modelo);
            String vId_Modelo;

            EquipamentosDTO Resumo_Dispositivo = new EquipamentosDTO();

            //Cso não exista o modelo cria-se com um fabricante desconhecido
            if (String.IsNullOrEmpty(pesquisa_modelo.Id_Modelo) == true)
            {
                // cria modelo
                vId_Modelo = _ModeloRepository.Cria_modelo(Modelo, vId_fabricante);

            }
            else
            {
                vId_Modelo = pesquisa_modelo.Id_Modelo;
            }

            Resumo_Dispositivo.IP = IP;
            Resumo_Dispositivo.maq_contrato = contrato;
            Resumo_Dispositivo.Nr_Serie = NS;
            Resumo_Dispositivo.Id_cliente = vId_Cliente;
            Resumo_Dispositivo.Modelo = vId_Modelo;


            // Criar dispositivo

            // Problema : Uma vez criado o equipamento ele pede que tenha um oid atrelado pelo menos na modelsOids
            // pois na hora de inserir uma leitura se vier nulo da uma quebra de chave para o id_oid
            // tem 2 caminhos 1 ao criar o equipamento criar um OID padrao para ele ou quando o id vier nulo 
            // adotar um id padrao.



            var vId_Dispositivo = _repositoryDispositivo.Cria_Dispositivo(Resumo_Dispositivo);


            return vId_Dispositivo;

        }


    }
}
