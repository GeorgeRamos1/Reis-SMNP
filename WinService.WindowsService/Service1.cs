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


namespace WinService.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        EquipamentosBusiness _BLLEquipamentos = new EquipamentosBusiness();
        SetupBusiness _BLLSetup = new SetupBusiness();
        LeituraBusiness _BllLeitura = new LeituraBusiness();




        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            EventLog.WriteEntry("teste de Serviço inicializado", EventLogEntryType.Warning);

            //cria o que será executado
            ThreadStart iniciarExecucao = new ThreadStart(varrerArede);

            //estancia a linha de execução
            Thread linhaDeExecucao = new Thread(iniciarExecucao);


            //rodar serviço que será executado
            linhaDeExecucao.Start();

        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("teste de Serviço Interrompido", EventLogEntryType.Warning);
        }


        // 1000= 1 segundo
        public void varrerArede()
        {

            while (true)
            {
                //    var Interval = _BLLSetup.Interval_Varredura();

                this.InicializaVariaveisGlobais();

                Thread.Sleep(Global.Ciclo);


                //Captura a leitura na Rede
                var Lista_Capturada = _BllLeitura.Captura_Leitura_Na_Rede();

                //Insere na Tabela
                _BllLeitura.Grava_Leitura_No_DB(Lista_Capturada);

            }


        }

        public void InicializaVariaveisGlobais()
        {

            //Tempo do ciclo de leitura 1s=1000
            var Interval = Properties.Settings.Default.ciclo;
            if (String.IsNullOrEmpty(Interval))
            {
                Global.Ciclo = 1000;
            }
            else
            {
                Global.Ciclo = Convert.ToInt32(Interval);
            }

            // Faixa de Ip
            var Faixa = Properties.Settings.Default.faixa_ip;
            if (String.IsNullOrEmpty(Faixa))
            {
                Global.Faixa_Ip = "192.168.0";
            }
            else
            {
                Global.Faixa_Ip = Faixa;
            }

            //Inicio da faixa de ip
            var Ini = Properties.Settings.Default.faixa_ini;

            if (String.IsNullOrEmpty(Ini))
            {
                Global.Inicio_faixa = 10;
            }
            else
            {
                Global.Inicio_faixa = Convert.ToInt32(Ini);
            }

            //fim da faixa de ip
            var Fim = Properties.Settings.Default.faixa_fim;

            if (String.IsNullOrEmpty(Fim))
            {
                Global.Fim_faixa = 255;
            }
            else
            {
                Global.Inicio_faixa = Convert.ToInt32(Fim);
            }

            //Oid do Numero Serie
            var Oid_Sn = Properties.Settings.Default.oid_serial;
            if (String.IsNullOrEmpty(Oid_Sn))
            {
                Global.Oid_serial = "1.3.6.1.2.1.43.5.1.1.17.1";
            }
            else
            {
                Global.Oid_serial = Oid_Sn;
            }

            //Oid do modelo
            var Oid_Model = Properties.Settings.Default.oid_modelo;
            if (String.IsNullOrEmpty(Oid_Model))
            {
                Global.Oid_modelo = "1.3.6.1.2.1.25.3.2.1.3.1";
            }
            else
            {
                Global.Oid_modelo = Oid_Model;
            }

            //Oid do serial
            var Oid_Mac = Properties.Settings.Default.oid_mac;
            if (String.IsNullOrEmpty(Oid_Mac))
            {
                Global.Oid_mac = "1.3.6.1.2.1.2.2.1.6.1";
            }
            else
            {
                Global.Oid_mac = Oid_Mac;
            }

            //Oid para definir se é impressora
            var Oid_e_impressora = Properties.Settings.Default.oid_impressora;
            if (String.IsNullOrEmpty(Oid_e_impressora))
            {
                Global.Oid_tipo_impressora = "1.3.6.1.2.1.43.5.1.1.1.1";
            }
            else
            {
                Global.Oid_tipo_impressora = Oid_e_impressora;
            }

            //Oid para contador geral
            var Oid_contador = Properties.Settings.Default.oid_contador;
            if (String.IsNullOrEmpty(Oid_contador))
            {
                Global.Oid_contador_geral = "1.3.6.1.2.1.43.10.2.1.4.1.1";
            }
            else
            {
                Global.Oid_contador_geral = Oid_contador;
            }

            

 //Id do cliente
            var Id_Cliente1 = Properties.Settings.Default.Id_cliente;
            if (String.IsNullOrEmpty(Id_Cliente1))
            {
                Global.Id_cliente = "6aa3d5f0-9056-11e6-bc34-00155d019604";
            }
            else
            {
                Global.Id_cliente = Oid_contador;
            }


        }
    }
}
