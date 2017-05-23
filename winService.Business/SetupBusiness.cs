using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinService.DTO;
using WinService.Utilities;
using System.Threading;
using System.Net;
using System.Windows.Forms;

using WinService.Repository;
using WinService.Utilities;

namespace WinService.Business
{
    public class SetupBusiness
    {
        LogEvento _log = new LogEvento();

        public Int32 Interval_Varredura()
        {
            return 60000;
        }

        Arquivos _UtilArq = new Arquivos();



        //Valida o licenciamento do programa
        public bool LicenciaPRG()
        {
            Boolean Status = false;
            // Busca os Arquivos de Envio e retorno
            try
            {

               
                String StrREQ = Global.Path_Cert + "\\Licenca.REQ";
                String StrLIC = Global.Path_Cert + "\\Licenca.LIC";
                String IP = Global.IP_LIC;

                var ExisteREQ = File.Exists(StrREQ);
                var ExisteLIC = File.Exists(StrLIC);

                //Define a linha a encriptar
                //nomedamaquina;dominiodamaquina;ip;dataehoradageração;nrcontrato;true
                String NomeDaMaquina = SystemInformation.ComputerName;
                String DominioUsuario = Environment.UserDomainName;
               


                TripleDES.setPassword("654321");

                if (ExisteLIC)
                {
                    // Se o Arq LIC Existe testa sua validade

                    var textoLIC = _UtilArq.LeArquivo(StrLIC);
                    //nomedamaquina;dominiodamaquina;ip;dataehoradageração;nrcontrato;validade
                    var textoLICDectyp = TripleDES.Decrypt(textoLIC);

                    string[] Certificado = textoLICDectyp.Split(';');

                    String NomePCLic = Certificado[0];
                    String DominioCLic = Certificado[1];
                    String IPLiC = Certificado[2];
                    String DataCriacaoLic = Certificado[3];
                    String NrContratoLic = Certificado[4];
                    String ValidadeLic = Certificado[5];


                    // Se o Ip existir Valida caso contrario desabilita
                    if (this.ValidaIp(IPLiC))
                    {
                        //Valida o nome da maquina
                        if (NomePCLic == NomeDaMaquina)
                        {

                            //VAlida o dominio da maquina
                            if (DominioCLic == DominioUsuario)
                            {


                                //Valida validade da licença
                                if (Convert.ToDateTime(ValidadeLic) >= DateTime.Now)
                                {
                                    Status = true;
                                }
                                else
                                {
                                    _log.WriteEntry("Erro ao validar Licença: Licença Vencida: " + ValidadeLic, System.Diagnostics.EventLogEntryType.Error);
                                }



                            }
                            else
                            {
                                _log.WriteEntry("Erro ao validar Licença: Domínio PC: " + DominioUsuario + " Está Incorreto", System.Diagnostics.EventLogEntryType.Error);
                            }



                        }
                        else
                        {
                            _log.WriteEntry("Erro ao validar Licença: Nome PC: " + NomeDaMaquina + " Está Incorreto", System.Diagnostics.EventLogEntryType.Error);
                        }



                    }
                    else
                    {
                        _log.WriteEntry("Erro ao validar Licença: IP da Maquina Invalido", System.Diagnostics.EventLogEntryType.Error);

                    }




                }
                else// Arquivo LIC não existe assim cria-se o Arquivo REQ
                {






                    String linha = NomeDaMaquina.Trim() + ";" + DominioUsuario.Trim() + ";" + IP + ";" + DateTime.Now.ToString() + ";" + Global.Nr_contrato.Trim() + ";true";
                    String LinhaCyto = TripleDES.Encrypt(linha);

                    //Gera o Arquivo REQ
                    _UtilArq.EscreveArquivo(StrREQ, LinhaCyto);

                }
            }
            catch (Exception ex)
            {

                _log.WriteEntry("Erro Na validação da licença: " + ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }

            //    _log.WriteEntry("Arquivo Licenca Não Encontrado", System.Diagnostics.EventLogEntryType.Error);



            return Status;
        }

        private Boolean ValidaIp(String Ip)
        {
            Boolean IpValidado = false;

            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            for (int i = 0; i < ip.Length; i++)
            {

                if (Ip == ip[i].ToString())
                {
                    IpValidado = true;
                }


            }

            return IpValidado;
        }


        ConfiguracaoRepository _configuracao = new ConfiguracaoRepository();

        public ConfiguracaoDTO pesquisaConfiguracao()
        {
            return _configuracao.ListarConfiguracao(1);

        }


    }
}
