using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WinService.Business;
using System.Threading;
using WinService.DTO;
using System.Configuration;
using System.ServiceProcess;
using WinService.Utilities;
namespace WinService.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        LogEvento _log = new LogEvento();
        EquipamentosBusiness _BLLEquipamentos = new EquipamentosBusiness();
        SetupBusiness _BLLSetup = new SetupBusiness();
        LeituraBusiness _BllLeitura = new LeituraBusiness();




        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            EventLog.WriteEntry("Serviço captura de OID iniciado", EventLogEntryType.Warning);

            //cria o que será executado
            ThreadStart iniciarExecucao = new ThreadStart(varrerArede);

            //estancia a linha de execução
            Thread linhaDeExecucao = new Thread(iniciarExecucao);


            //rodar serviço que será executado
            linhaDeExecucao.Start();

        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Serviço captura de OID Interrompido", EventLogEntryType.Warning);

        }


        // 1000= 1 segundo
        public void varrerArede()
        {
            _log.WriteEntry("Varredura da Rede Iniciada :", System.Diagnostics.EventLogEntryType.Information);
            while (true)
            {
                //    var Interval = _BLLSetup.Interval_Varredura();

                this.InicializaVariaveisGlobais();

                Thread.Sleep(Global.Ciclo);



                //Aqui introduzimos o licienciamento.
                Boolean ValidaLicienciamento = _BLLSetup.LicenciaPRG();

                if (ValidaLicienciamento)
                {

                    //Captura a leitura na Rede
                    var Lista_Capturada = _BllLeitura.Captura_Leitura_Na_Rede();

                    //Insere na Tabela
                    _BllLeitura.Grava_Leitura_No_DB(Lista_Capturada);
                }
                else
                {
                    Service1 servico = new Service1();
                    servico.Stop();
                    _log.WriteEntry("Varredura Interrompida por falta de licença :", System.Diagnostics.EventLogEntryType.Information);
                }
               
            }


        }

        public void InicializaVariaveisGlobais()
        {

            //Tempo do ciclo de leitura 1s=1000
            var Interval = ConfigurationManager.AppSettings["ciclo"];
            if (String.IsNullOrEmpty(Interval))
            {
                Global.Ciclo = 1000;
            }
            else
            {
                Global.Ciclo = Convert.ToInt32(Interval);
            }

            // Faixa de Ip

            var Faixa = ConfigurationManager.AppSettings["faixa_ip"];
            // var Faixa = Properties.Settings.Default.faixa_ip;
            if (String.IsNullOrEmpty(Faixa))
            {
                Global.Faixa_Ip = "192.168.0";
            }
            else
            {
                Global.Faixa_Ip = Faixa;
            }

            //Inicio da faixa de ip
            var Ini = ConfigurationManager.AppSettings["faixa_ini"];

            if (String.IsNullOrEmpty(Ini))
            {
                Global.Inicio_faixa = 10;
            }
            else
            {
                Global.Inicio_faixa = Convert.ToInt32(Ini);
            }

            //fim da faixa de ip
            var Fim = ConfigurationManager.AppSettings["faixa_fim"];

            if (String.IsNullOrEmpty(Fim))
            {
                Global.Fim_faixa = 255;
            }
            else
            {
                Global.Fim_faixa = Convert.ToInt32(Fim);
            }

            //Oid do Numero Serie
            var Oid_Sn = ConfigurationManager.AppSettings["oid_serial"];
            if (String.IsNullOrEmpty(Oid_Sn))
            {
                Global.Oid_serial = "1.3.6.1.2.1.43.5.1.1.17.1";
            }
            else
            {
                Global.Oid_serial = Oid_Sn;
            }

            //Oid do modelo
            var Oid_Model = ConfigurationManager.AppSettings["oid_modelo"];
            if (String.IsNullOrEmpty(Oid_Model))
            {
                Global.Oid_modelo = "1.3.6.1.2.1.25.3.2.1.3.1";
            }
            else
            {
                Global.Oid_modelo = Oid_Model;
            }

            //Oid do serial
            var Oid_Mac = ConfigurationManager.AppSettings["oid_mac"];
            if (String.IsNullOrEmpty(Oid_Mac))
            {
                Global.Oid_mac = "1.3.6.1.2.1.2.2.1.6.1";
            }
            else
            {
                Global.Oid_mac = Oid_Mac;
            }

            //Oid para definir se é impressora
            var Oid_e_impressora = ConfigurationManager.AppSettings["oid_impressora"];
            if (String.IsNullOrEmpty(Oid_e_impressora))
            {
                Global.Oid_tipo_impressora = "1.3.6.1.2.1.43.5.1.1.1.1";
            }
            else
            {
                Global.Oid_tipo_impressora = Oid_e_impressora;
            }

            //Oid para contador geral
            var Oid_contador = ConfigurationManager.AppSettings["oid_contador"];
            if (String.IsNullOrEmpty(Oid_contador))
            {
                Global.Oid_contador_geral = "1.3.6.1.2.1.43.10.2.1.4.1.1";
            }
            else
            {
                Global.Oid_contador_geral = Oid_contador;
            }



            //Id do cliente
            var Id_Cliente1 = ConfigurationManager.AppSettings["Id_cliente"];
            if (String.IsNullOrEmpty(Id_Cliente1))
            {
                Global.Id_cliente = "99b062f8-d415-11e6-bfdc-00155d6eac04";
            }
            else
            {
                Global.Id_cliente = Id_Cliente1;
            }
            //Id do cliente
            var Tempo_espera = ConfigurationManager.AppSettings["TimeOut_snpm"];
            if (String.IsNullOrEmpty(Tempo_espera))
            {
                Global.TimeOut = "300";
            }
            else
            {
                Global.TimeOut = Tempo_espera;
            }
            //String de conexão
            var conn = ConfigurationManager.AppSettings["Conn_db"];
            if (String.IsNullOrEmpty(conn))
            {
                EventLog.WriteEntry("String de conexão não definida", EventLogEntryType.Warning);

            }

            Global.Path_Cert = ConfigurationManager.AppSettings["Path_Cert"];

            Global.Nr_contrato = ConfigurationManager.AppSettings["Nr_contrato"];

            Global.IP_LIC = ConfigurationManager.AppSettings["IP"];




        }






    }
}
