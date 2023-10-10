using Bunkum.RealmDatabase;
using Furukawa.Types;
using Realms;

namespace Furukawa.Database;

public class FurukawaDatabaseProvider : RealmDatabaseProvider<FurukawaDatabaseContext>
{
    public FurukawaDatabaseProvider()
    {
    }

    public override void Warmup()
    {
        //throw new NotImplementedException();
        // TODO: Make this not shit
        // Check to see if there are any new notes to merge
        /*
        CorpusNote note = new();
        note.SortField = "愛";
        note.Content.Add(new KeyValuePair<string, string>("answer", "愛"));
        note.Template = "test";

        FSRSharp.Card card = new();
        FsrsCard dbCard = new();
        dbCard.Guid = new Guid();
        dbCard.Owner = "Logan";
        dbCard.Note = "愛";

        dbCard = dbCard.UpdateCard(card);

        this.GetContext().AddSomeCorpusNotesTest(note);
        this.GetContext().WriteFsrsRealmCard(dbCard);
        */
    }

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        //throw new NotImplementedException();
    }

    protected override ulong SchemaVersion { get; }

    protected override List<Type> SchemaTypes { get; } = new()
    {
        typeof(SiteUser),
        typeof(SiteSession),
        typeof(UserStatistics),
        typeof(FsrsCard),
        typeof(FsrsReviewLog),
        typeof(CorpusNote),
        typeof(CorpusStat),
        typeof(CorpusTemplate)
    };
    protected override string Filename => "furukawa.realm";
}