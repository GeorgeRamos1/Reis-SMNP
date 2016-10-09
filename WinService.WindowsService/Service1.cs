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
                var Interval = _BLLSetup.Interval_Varredura();
                Thread.Sleep(Interval);


                //Captura a leitura na Rede
                var Lista_Capturada = _BllLeitura.Captura_Leitura_Na_Rede();

                //Insere na Tabela
                _BllLeitura.Insere_Leitura(Lista_Capturada);

            }


        }
    }
}
