using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace WinService.Utilities
{
    public class LogEvento
    {

        const string LOG_NAME = "Application"; //Nome da entrada no Visualizador de Eventos do Windows 
        const string SOURCE = "CapturaOID_SMNP"; //Nome da fonte (source) com que serão gravados os logs. 

        public LogEvento()
        {
            //verifica se o source log já existe, se não existe então cria;  
            if (EventLog.SourceExists(SOURCE) == false)
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
        }

        public void WriteEntry(string input, EventLogEntryType entryType)
        {
            //grava o texto na fonte de logs com o nome que      definimos para a constante SOURCE.  
            EventLog.WriteEntry(SOURCE, input, entryType);
        }

        public void WriteEntry(string input)
        {
            //loga um simples evento com a categoria de informação.  
            WriteEntry(input, EventLogEntryType.Information);
        }

        public void WriteEntry(Exception ex)
        {
            //loga a ocorrência de uma excessão com a categoria de erro.  
            WriteEntry(ex.ToString(), EventLogEntryType.Error);
        }
    }
}
