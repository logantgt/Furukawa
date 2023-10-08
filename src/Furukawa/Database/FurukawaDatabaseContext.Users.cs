using Furukawa.Requests.Account;
using Furukawa.Types;

namespace Furukawa.Database;

public partial class FurukawaDatabaseContext
{
    public SiteUser CreateUser(RegistrationRequest request)
    {
        SiteUser user = new()
        {
            Id = GenerateGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordBcrypt = BCrypt.Net.BCrypt.HashPassword(request.PasswordSha512.ToLower(), WorkFactor),
            CreationDate = DateTimeOffset.UtcNow,
            Statistics = new UserStatistics()
        };

        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
    }

    public void RemoveUser(SiteUser user)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Sessions);
            _realm.Remove(user);
        });
    }

    public void SetUsername(SiteUser user, string username)
    {
        _realm.Write(() =>
        {
            user.Username = username;
        });
    }
    
    public void SetUserEmail(SiteUser user, string email)
    {
        _realm.Write(() =>
        {
            user.Email = email;
        });
    }
    
    public void SetUserSelectedSkin(SiteUser user, int selectedSkin)
    {
        _realm.Write(() =>
        {
            user.SelectedSkin = selectedSkin;
        });
    }
    
    public void SetUserStatistics(SiteUser user, UserStatistics statistics)
    {
        _realm.Write(() =>
        {
            user.Statistics = statistics;
        });
    }
    
    private const int WorkFactor = 10;
    public bool ValidatePassword(SiteUser user, string hash)
    {
        if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
        {
            SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor));
        }

        return BCrypt.Net.BCrypt.Verify(hash.ToLower(), user.PasswordBcrypt); 
    }

    public void SetUserPassword(SiteUser user, string hash)
    {
        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor);
        
        _realm.Write(() =>
        {
            user.PasswordBcrypt = passwordBcrypt;
        });
    }
    
    public SiteUser? GetUserWithEmail(string email)
    {
        return _realm.All<SiteUser>().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
    
    public SiteUser? GetUserWithId(string id)
    {
        return _realm.All<SiteUser>().FirstOrDefault(u => u.Id == id);
    }
        
    public SiteUser? GetUserWithUsername(string username)
    {
        return _realm.All<SiteUser>().FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
}