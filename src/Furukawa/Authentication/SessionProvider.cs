using Bunkum.Protocols.Http.Direct;
using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using Bunkum.Listener.Request;
using Furukawa.Database;
using Furukawa.Types;
using static Furukawa.Helpers.SessionHelper;

namespace Furukawa.Authentication;

public class SessionProvider : IAuthenticationProvider<SiteSession>
{
    public SiteUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        SiteUser? user = AuthenticateToken(request, db)?.User;
        if (user == null) return null;

        user.RateLimitUserId = user.Id;
        return user;
    }

    public SiteSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? id = request.RequestHeaders["Authorization"];
        if (id == null) return null;

        FurukawaDatabaseContext database = (FurukawaDatabaseContext)db.Value;

        SiteSession? session = database.GetSessionWithId(id);
        if (session == null) return null;

        if (session.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveSession(session);
            return null;
        }
        
        if (!IsSessionAllowedToAccessEndpoint(session, request.Uri.AbsolutePath)) return null;
        return session;
    }
}