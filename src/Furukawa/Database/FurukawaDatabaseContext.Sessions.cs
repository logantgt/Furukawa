using Furukawa.Types;

namespace Furukawa.Database;

public partial class FurukawaDatabaseContext
{
    private const int DefaultSessionExpirySeconds = 86400; // 1 day
    private const int SessionLimit = 3; // limit of how many sessions a user can have simultaneously 
    public SiteSession CreateSession(SiteUser user, SessionType type, long? expirationSeconds = null, string? id = null)
    {
        double sessionExpirationSeconds = expirationSeconds ?? DefaultSessionExpirySeconds;
        
        SiteSession session = new()
        {
            Id = id ?? GenerateGuid(),
            SessionType = type,
            User = user,
            CreationDate = DateTimeOffset.UtcNow,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds)
        };

        IEnumerable<SiteSession> sessionsToDelete = _realm.All<SiteSession>()
            .Where(s => s.User == user && s._SessionType == (int)type)
            .AsEnumerable()
            .SkipLast(SessionLimit - 1);

        _realm.Write(() =>
        {
            foreach (SiteSession gameSession in sessionsToDelete)
            {
                _realm.Remove(gameSession);
            }
            
            _realm.Add(session);
        });
        
        return session;
    }
    public void RemoveSession(SiteSession session)
    {
        _realm.Write(() =>
        {
            _realm.Remove(session);
        });
    }
    public SiteSession? GetSessionWithId(string id)
    {
        return _realm.All<SiteSession>().FirstOrDefault(s => s.Id == id);
    }
}