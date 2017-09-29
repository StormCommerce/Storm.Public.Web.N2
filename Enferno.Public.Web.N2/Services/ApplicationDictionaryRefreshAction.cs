
using System;
using Enferno.Public.Logging;
using N2.Plugin.Scheduling;

namespace Enferno.Public.Web.N2.Services
{
    [ScheduleExecution(60, TimeUnit.Minutes, Repeat = Repeat.Indefinitely)]
    public class ApplicationDictionaryRefreshAction : ScheduledAction
    {
        public override void Execute()
        {
            Log.LogEntry.Categories(CategoryFlags.Debug).Message("Processing ApplicationDictionary refresh").WriteVerbose();
            ApplicationDictionary.Instance.Refresh();
        }

        public override void OnError(Exception ex)
        {
            Log.LogEntry.Message("Error in {0}.{1}: {2}", this.GetType().Name, "Execute", ex.Message)
                .Categories(CategoryFlags.Alert)
                .Exceptions(ex)
                .WriteError();
        }
    }
}
