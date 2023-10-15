using AnkiNet;
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
        // throw new NotImplementedException();
        // Import Anki Decks found in merge folder
        foreach (string file in Directory.GetFiles("merge", "*.apkg"))
        {
            ImportAnkiDeck($"{file}");
        }
    }

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        // throw new NotImplementedException();
    }

    private async Task ImportAnkiDeck(string file)
    {
        string templatename = "";
        AnkiCollection collection = await AnkiFileReader.ReadFromFileAsync(file);
        foreach (AnkiNoteType template in collection.NoteTypes)
        {
            templatename = template.Name;
            CorpusTemplate fkTemplate = new CorpusTemplate();
            fkTemplate.Name = template.Name;
            fkTemplate.Content = "<style>";
            fkTemplate.Content += template.Css;
            fkTemplate.Content += "</style>";
            fkTemplate.Content += "<div id=\"card-front\">";
            fkTemplate.Content += template.CardTypes[0].QuestionFormat;
            fkTemplate.Content += "</div>";
            fkTemplate.Content += "<div id=\"card-back\">";
            fkTemplate.Content += template.CardTypes[0].AnswerFormat;
            fkTemplate.Content += "</div>";
            
            this.GetContext().WriteCorpusTemplate(fkTemplate);
        }

        int order = 0;
        
        foreach (AnkiCard card in collection.Decks[1].Cards)
        {
            Dictionary<string, string> fields = new();
            int index = 0;
            
            foreach (string field in card.Note.Fields)
            {
                fields.Add(collection.NoteTypes[0].FieldNames[index], field);
                index++;
            }

            CorpusNote note = new(fields.First().Value, fields, templatename, new string[]{});

            note.Guid = Guid.NewGuid();
            
            note.Order = order;

            note.Deck = collection.Decks[1].Name;

            this.GetContext().AddSomeCorpusNotesTest(note);
            
            order++;
        }

    }

    protected override ulong SchemaVersion { get; }

    protected override List<Type> SchemaTypes { get; } = new()
    {
        typeof(SiteUser),
        typeof(SiteSession),
        typeof(UserStatistics),
        typeof(RealmCard),
        typeof(RealmReviewLog),
        typeof(CorpusNote),
        typeof(CorpusTemplate)
    };
    protected override string Filename => "furukawa.realm";
}