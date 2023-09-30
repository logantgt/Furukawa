using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using Furukawa.Database;
using Furukawa.Types;
using static Furukawa.Helpers.SessionHelper;

namespace Furukawa.Authentication
{
    public class SessionProvider : IAuthenticationProvider<GameUser, GameSession>
    {
        public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
        {
            GameUser? user = AuthenticateToken(request, db)?.User;
            if (user == null) return null;

            user.RateLimitUserId = user.Id;
            return user;
        }

        public GameSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
        {
            string? id = request.RequestHeaders["Authorization"];
            if (id == null) return null;

            FurukawaDatabaseContext database = (FurukawaDatabaseContext)db.Value;

            GameSession? session = database.GetSessionWithId(id);
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
}