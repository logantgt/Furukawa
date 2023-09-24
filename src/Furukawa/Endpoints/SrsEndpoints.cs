using System.Net;
using System.Runtime.CompilerServices;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Furukawa.Database;
using Furukawa.Models;
using Furukawa.Services;
using FSRSharp;
using Furukawa.Types;

namespace Furukawa.Endpoints
{
    public class SrsEndpoints : EndpointGroup
    {
        [Endpoint("/api/v1/srs/QueryNextDueCard", Method.Get, ContentType.Json)]
        public CardContent QueryNextDueCard(RequestContext context, FurukawaDatabaseContext database)
        {
            // Return the UUID of the user's next card to the client along with the Card Contents for presentation
            FsrsRealmCard newCard = database.QueryFsrsRealmCardsDue();
            CardContent cardContent = new CardContent
            {
                CardGuid = newCard.Guid,
                Document = ""
            };

            return cardContent;
        }
        
        [Endpoint("/api/v1/srs/GradeCard", Method.Post)]
        public HttpStatusCode GradeCard(RequestContext context, FurukawaDatabaseContext database, FsrsAlgorithm fsrs, string body)
        {
            // Accept card UUID and grade, update card in database and publish review log
            return HttpStatusCode.OK;
        }
        
        // "/api/v1/srs/QueryDueCardCount" (get)
        // "/api/v1/srs/GradeCard" (post)
    }    
}
