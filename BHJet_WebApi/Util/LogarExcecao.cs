using System;
using System.Diagnostics;
using System.Web.Http.ExceptionHandling;

namespace BHJet_WebApi.Util
{
    public class LogarExcecao : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            // Log Event Viewer
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("Mensagem: " + context.Exception.Message + Environment.NewLine +
                                    "Inner: " + context.Exception.InnerException + Environment.NewLine +
                                    "Stack: " + context.Exception.StackTrace, EventLogEntryType.Error, 101, 1);
            }

            // Trace Error
            Trace.TraceError(context.ExceptionContext.Exception.ToString());
        }
    }
}