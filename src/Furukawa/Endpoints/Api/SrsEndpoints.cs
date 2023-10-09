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
using Mustache;

namespace Furukawa.Endpoints.Api;

public class SrsEndpoints : EndpointGroup
{
    [ApiEndpoint("srs/NextDueCard", HttpMethods.Get)]
    public RenderedCard NextDueCard(RequestContext context, FurukawaDatabaseContext database)
    {
        // Send the UUID of the user's next card to the client along with the Card Contents for presentation
        FsrsCard newCard = database.QueryNextCard();
        RenderedCard renderedCard = new RenderedCard()
        {
            Guid = Guid.NewGuid().ToString(),
            Content = RenderCard(database.QueryNote(newCard.Note), database)
        };

        return renderedCard;
    }

    [ApiEndpoint("srs/GradeCard", HttpMethods.Post)]
    public HttpStatusCode GradeCard(RequestContext context, FurukawaDatabaseContext database, FsrsAlgorithm fsrs, string body)
    {
        // Accept card UUID and grade, update card in database and publish review log
        return HttpStatusCode.OK;
    }
        
    [ApiEndpoint("srs/DueCardCount", HttpMethods.Post)]
    public HttpStatusCode DueCardCount(RequestContext context, FurukawaDatabaseContext database, FsrsAlgorithm fsrs, string body)
    {
        return HttpStatusCode.OK;
    }

    private string RenderCard(CorpusNote note, FurukawaDatabaseContext database)
    {
        return Template.Compile(database.ReadCorpusTemplate(note.Template)).Render(note.Content);
    }
}