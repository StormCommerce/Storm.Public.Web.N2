
using Enferno.Public.Web.N2.Items.Pages;

namespace Enferno.Public.Web.N2.Utilities
{
    public class N2CertificateResolver : StormApiClient.CertificateResolver
    {
        public N2CertificateResolver()
        {
            var applicationRoot = PageHelper.FindApplicationRoot() as IApplicationRoot;
            if (applicationRoot != null && !string.IsNullOrWhiteSpace(applicationRoot.CertificatePath))
            {
                this.File = applicationRoot.CertificatePath;
            }
        }
    }
}
