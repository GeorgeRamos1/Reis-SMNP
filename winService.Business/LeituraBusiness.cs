﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinService.DTO;
using WinService.Repository;
using WinService.Utilities;


namespace WinService.Business
{
    public class LeituraBusiness
    {
        //Camada de negocios do objeto leitura
        Snmp _SNMP = new Snmp();
        EquipamentosBusiness _BLLEquipamentos = new EquipamentosBusiness();
        Leitura_Repository _repository_leitura = new Repository.Leitura_Repository();

        public void Grava_Leitura_No_DB(IEnumerable<OID_Leitura> Leituras_Dispositivos)
        {


            foreach (var leitura in Leituras_Dispositivos)
            {
                //se nã encontrar o OID usa o OID geral
                //if (String.IsNullOrEmpty(leitura.OID))
                //{
                //    //tratar exceção
                //    leitura.OID = "1.3.6.1.2.1.43.10.2.1.4.1.1";
                //}
                var retorno_inserir = _repository_leitura.Insere_Leitura(leitura);


                //criar um log de falha no caso de erro na inserção


            }






        }


        public IEnumerable<OID_Leitura> Captura_Leitura_Na_Rede()
        {
            //Regra: caso seja um dispositvo valido (Impressora), verifica se existe na lista de equipamentos
            //Se for um equipamento de contrato busca modelo/Matriz, se não existir na lista cria com equipamento de outro
            //usa a lista/matriz generica

            //procura impressoras na rede
            var Lista_dispositivo_Encontrados = _BLLEquipamentos.Captura_Ip_Dipositivos_Na_rede();

            //Valida os OIDs na tabela oids, se não existir usa oid padrão
            var Lista_OIDs_Capturados = _BLLEquipamentos.Captura_OID_Dispositivo(Lista_dispositivo_Encontrados);

            // Faz a leitura dos oids na rede por dispositivo
            var ListaValoresOidLidos = Busca_Valores_De_OID_Dispositivos(Lista_OIDs_Capturados);

            return ListaValoresOidLidos;

        }



        public IEnumerable<OID_Leitura> Busca_Valores_De_OID_Dispositivos(IEnumerable<OIDs_Dispositivo> Lista_Dispositivo)
        {

            var ListaOidLidos = new List<OID_Leitura>();

            foreach (var Dispositivo in Lista_Dispositivo)
            {
                var Lista_OIDs = new OID_Leitura();

                //Captura o Valor
                var vLeitura = _SNMP.capturaOID(Dispositivo.IP, Dispositivo.OID);

                Lista_OIDs.IP = Dispositivo.IP;
                Lista_OIDs.Nr_Serie = Dispositivo.Nr_Serie;
                // Lista_OIDs.maq_contrato = Dispositivo.maq_contrato;
                //  Lista_OIDs.Descricao = Dispositivo.Descricao;
                Lista_OIDs.OID = Dispositivo.OID;
                Lista_OIDs.Valor = vLeitura.Valor;
                Lista_OIDs.Id_equipamento = Dispositivo.Id_equipamento;
                ListaOidLidos.Add(Lista_OIDs);
            }

            return ListaOidLidos;
        }
    }
}
