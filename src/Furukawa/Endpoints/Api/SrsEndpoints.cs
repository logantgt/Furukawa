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
using Furukawa.Requests;
using Furukawa.Types;
using Mustache;
using Newtonsoft.Json;

namespace Furukawa.Endpoints.Api;

public class SrsEndpoints : EndpointGroup
{
    [ApiEndpoint("srs/NextDueCard", HttpMethods.Get, ContentType.Json)]
    public RenderedCard NextDueCard(RequestContext context, FurukawaDatabaseContext database)
    {
        // Send the UUID of the user's next card to the client along with the Card Contents for presentation
        FsrsCard newCard = database.QueryNextCard();
        RenderedCard renderedCard = new RenderedCard()
        {
            Guid = newCard.Guid.ToString(),
            State = newCard.State,
            Content = RenderCard(database.QueryNote(newCard.Note), database)
        };

        return renderedCard;
    }

    [ApiEndpoint("srs/GradeCard", HttpMethods.Post, ContentType.Json)]
    public HttpStatusCode GradeCard(RequestContext context, FurukawaDatabaseContext database, CardGradeRequest body, FsrsAlgorithm fsrs)
    {
        // Accept card UUID and grade, update card in database and publish review log
        FsrsCard grading = database.GetFsrsCardByGuid(Guid.Parse(body.Guid));
        SchedulingInfo info = fsrs.Repeat(grading.ToCard())[(CardRating)body.Grade];

        grading = grading.UpdateCard(info.Card);
        
        database.WriteFsrsRealmCard(grading);
        database.WriteFsrsRealmReviewLog(FsrsReviewLog.FromReviewLog(info.FsrsReviewLog, Guid.Parse(body.Guid)));
        
        return HttpStatusCode.OK;
    }
        
    [ApiEndpoint("srs/DueCardCount", HttpMethods.Get)]
    public HttpStatusCode DueCardCount(RequestContext context, FurukawaDatabaseContext database, FsrsAlgorithm fsrs)
    {
        return HttpStatusCode.OK;
    }

    private string RenderCard(CorpusNote note, FurukawaDatabaseContext database)
    {
        return Template.Compile(database.ReadCorpusTemplate(note.Template)).Render(note.Content.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }
}