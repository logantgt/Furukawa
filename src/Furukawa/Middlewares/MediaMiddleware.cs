using System.Net;
using Bunkum.Core.Database;
using Bunkum.Core.Endpoints.Middlewares;
using Bunkum.Listener.Request;

namespace Furukawa.Middlewares;

public class MediaMiddleware : IMiddleware
{
    private static bool HandleMediaRequest(ListenerContext context)
    {
        if (!context.Uri.AbsolutePath.StartsWith("/media")) return false;
        
        string resource = context.Uri.Segments.Last().Replace("/", "");
        byte[] media;

        if (!FurukawaServer.DataStore.TryGetDataFromStore(resource, out media))
        {
            context.ResponseStream.Position = 0;
            context.ResponseCode = HttpStatusCode.NotFound;
            context.Write("404 Not Found");
            return true;
        }
        
        context.ResponseStream.Position = 0;
        context.ResponseCode = HttpStatusCode.OK;
        context.ResponseHeaders["Cache-Control"] = "max-age=43200";
        context.Write(media);
        return true;
    }

    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        if (!HandleMediaRequest(context)) next();
    }
}