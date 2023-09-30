using Furukawa.Database;
using Furukawa.Endpoints;
using Furukawa.Types;

namespace Furukawa.Helpers
{
    public static class SessionHelper
    {
        public static string GenerateEmailSessionId(FurukawaDatabaseContext database) =>
            GenerateSimpleSessionId(database, "123456789", 8);
        // ReSharper disable StringLiteralTypo    
        public static string GeneratePasswordSessionId(FurukawaDatabaseContext database) =>
            GenerateSimpleSessionId(database, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8);
        public static string GenerateAccountRemovalSessionId(FurukawaDatabaseContext database) => GenerateSimpleSessionId(database,
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 8);
        // ReSharper restore StringLiteralTypo
        // This is used for occasions where the user has to type the session id manually, and giving them a
        // SHA512 would be pretty inconvenient
        private static string GenerateSimpleSessionId(FurukawaDatabaseContext database, string idCharacters, int idLength)
        {
            Random r = new();
            string id = "";
            for (int i = 0; i < idLength; i++)
            {
                id += idCharacters[r.Next(idCharacters.Length - 1)];
            }

            if (database.GetSessionWithId(id) == null) return id; // Return if Id has not been used before
            return GenerateSimpleSessionId(database, idCharacters, idLength); // Generate new Id if it already exists   
        }
        public static bool IsSessionAllowedToAccessEndpoint(GameSession session, string uriPath)
        {
            if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute + "account/"))
            {
                switch (uriPath)
                {
                    case ApiEndpointAttribute.BaseRoute + "account/setEmail":
                        if (session.SessionType == SessionType.SetEmail) return true;
                        return false;
                    case ApiEndpointAttribute.BaseRoute + "account/setPassword":
                        if (session.SessionType == SessionType.SetPassword) return true;
                        return false;
                    case ApiEndpointAttribute.BaseRoute + "account/remove":
                        if (session.SessionType == SessionType.RemoveAccount) return true;
                        return false;
                }
            }
            
            if (session.SessionType == SessionType.Api && uriPath.StartsWith(ApiEndpointAttribute.BaseRoute)) return true;

            return false;
        }
    }
}