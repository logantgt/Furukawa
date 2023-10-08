using System.Net;
using System.Runtime.CompilerServices;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using Furukawa.Database;
using Furukawa.Models;
using Furukawa.Services;
using FSRSharp;
using Furukawa.Types;

namespace Furukawa.Endpoints.Api;

public class SrsEndpoints : EndpointGroup
{
    [ApiEndpoint("srs/QueryNextDueCard", HttpMethods.Get)]
    public CardContent QueryNextDueCard(RequestContext context, FurukawaDatabaseContext database)
    {
        // Send the UUID of the user's next card to the client along with the Card Contents for presentation
        //FsrsCard newCard = database.QueryNextCard();
        CardContent cardContent = new CardContent()
        {
            //Guid = newCard.Guid,
            //Content = File.ReadAllText(database.QueryNote(newCard.Note).Content),
            //Template = "<p>yeah Bitch</p>"
            Guid = Guid.NewGuid().ToString(),
            Content = "yas bros so contentufl",
            Template = "<p>yeah Bitch</p>"
        };

        return cardContent;
    }
        
        
    [ApiEndpoint("srs/GradeCard", HttpMethods.Post)]
    public HttpStatusCode GradeCard(RequestContext context, FurukawaDatabaseContext database, FsrsAlgorithm fsrs, string body)
    {
        // Accept card UUID and grade, update card in database and publish review log
        return HttpStatusCode.OK;
    }
        
    // "/api/v1/srs/QueryDueCardCount" (get)
    // "/api/v1/srs/GradeCard" (post)
}