
using Enferno.Web.StormUtils;
using N2.Engine;
using N2.Plugin;
using N2.Web;

namespace Enferno.Public.Web.N2.Services
{
    [Service]
    public class StormModuleInitializer : IAutoStart
    {
        private StormModule module;
        #region IStartable Members

        public void Start()
        {
            var broker = EventBroker.Instance;
            module = new StormModule();

            broker.BeginRequest += module.BeginRequest;
            broker.EndRequest += module.EndRequest;
        }

        public void Stop()
        {
            var broker = EventBroker.Instance;

// ReSharper disable once DelegateSubtraction
            broker.BeginRequest -= module.BeginRequest;
// ReSharper disable once DelegateSubtraction
            broker.EndRequest -= module.EndRequest;
        }

        #endregion
    }
}
