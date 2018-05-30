
using System;
using System.Linq;
using System.Web;
using Enferno.Public.Caching;
using Enferno.Public.InversionOfControl;
using N2.Edit;
using N2.Edit.FileSystem;
using N2.Edit.FileSystem.NH;
using N2.Engine;
using N2.Plugin;
using N2.Web;

using Unity;

namespace Enferno.Public.Web.N2.Services
{
    [Service(Configuration = "dbfs", Replaces = typeof(UploadFileHttpHandler))]
    public class DbfsHttpHandler : IHttpHandler, IAutoStart
    {
        private readonly IFileSystem fileSystem;
        private readonly UploadFolderSource folderSource;
        private readonly EventBroker broker;

        public DbfsHttpHandler(IFileSystem fileSystem, UploadFolderSource folderSource, EventBroker broker)
        {
            this.fileSystem = fileSystem;
            this.folderSource = folderSource;
            this.broker = broker;
        }

        public DbfsHttpHandler()
        {
        }

        private static string GetMimeTypeFromExtension(string path)
        {
            switch (VirtualPathUtility.GetExtension(path.ToLower()))
            {
                case ".css":
                    return "text/css";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".js":
                    return "text/javascript";
                case ".pdf":
                    return "application/pdf";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }

        #region IHttpHandler members
        public void ProcessRequest(HttpContext context)
        {
            DateTime updated;
            var cacheKey = context.Request.Url.AbsolutePath;
            var cache = IoC.Container.Resolve<ICache>("DBFS");

            var inCache = cache.TryGet(cacheKey, out updated);
            if (inCache && updated > DateTime.MinValue && !Modified(context, updated)) return;

            if (fileSystem == null) throw new HttpException(404, "File not found: " + context.Request.Path);
            var file = fileSystem.GetFile(context.Request.Path);
            if (file == null) throw new HttpException(404, "File not found: " + context.Request.Path);

            if (Modified(context, file.Updated))
            {
                context.Response.ContentType = GetMimeTypeFromExtension(context.Request.Path);
                context.Response.Cache.SetLastModified(file.Updated);
                context.Response.Cache.SetMaxAge(new TimeSpan(0, 30, 0));
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(10));
                fileSystem.ReadFileContents(context.Request.Path, context.Response.OutputStream);
            }

            if (!inCache || file.Updated > updated) cache.Add(cacheKey, file.Updated, 600);
        }

        private static bool Modified(HttpContext context, DateTime updated)
        {
            DateTime modifiedTime;
            DateTime.TryParse(context.Request.Headers["If-Modified-Since"], out modifiedTime);

            if (modifiedTime == DateTime.MinValue || modifiedTime < updated) return true;

            context.Response.StatusCode = 304;
            context.Response.StatusDescription = "Not Modified";
            return false;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private void HttpApplication_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null) return;


            var uploadFolders = folderSource.GetUploadFoldersForCurrentSite();
            if (!uploadFolders.Any(x => app.Request.Path.StartsWith(x.Path.TrimStart('~'), StringComparison.OrdinalIgnoreCase)))
                return;

            if (!fileSystem.FileExists(app.Request.Path))
                return;

            app.Context.Handler = this;
        }

        #endregion

        #region IAutoStart Members
        public void Start()
        {
            broker.PreRequestHandlerExecute += HttpApplication_PreRequestHandlerExecute;
        }

        public void Stop()
        {
// ReSharper disable once DelegateSubtraction
            broker.PreRequestHandlerExecute -= HttpApplication_PreRequestHandlerExecute;
        }

        #endregion
    }
}
