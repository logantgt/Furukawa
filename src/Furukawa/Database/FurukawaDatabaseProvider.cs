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
        string[] newNotesToMerge = Directory.GetFiles("merge/notes", "*.ini");

        if (newNotesToMerge.Length > 0)
        {
            Console.WriteLine("Found new notes! Merging...");
            foreach (string notePath in newNotesToMerge)
            {
                /*
                string card = File.ReadAllText(notePath);
    
                string sortField = card.Split("sortfield=")[1].Split("\n")[0];
                string[] dependencies = card.Split("dependencies=")[1].Split("\n")[0].Split("/");
                string html = card.Split("content=")[1];

                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine("content", $"{Path.GetFileNameWithoutExtension(notePath)}.html")))
                {
                    outputFile.Write(html);
                }
    
                CorpusNote newNote = new CorpusNote(sortField, $"content/{Path.GetFileNameWithoutExtension(notePath)}.html", dependencies);
    
                this.GetContext().AddSomeCorpusNotesTest(newNote);
    
                Console.WriteLine($"Added " + $"content/{Path.GetFileNameWithoutExtension(notePath)}.html");
                
                File.Delete(notePath);
                */
            }
        }
            
        string[] newListsToMerge = Directory.GetFiles("merge/lists", "*.ini");

        if (newListsToMerge.Length > 0)
        {
            Console.WriteLine("Found new lists! Merging...");
            foreach (string listPath in newListsToMerge)
            {
                string list = File.ReadAllText(listPath);

                string sourceDeck = list.Split("deck=")[1].Split("\n")[0];
                string[] cards = list.Split("[card]");

                foreach (string card in cards)
                {
                    if (card.Contains("deck=")) continue;
                        
                    string sortField = card.Split("sortfield=")[1].Split("\n")[0];
                    int order = int.Parse(card.Split("order=")[1].Split("\n")[0]);

                    CorpusStat newList = new CorpusStat
                    {
                        Deck = sourceDeck,
                        SortField = sortField,
                        Order = order,
                    };
        
                    this.GetContext().AddSomeCorpusListsTest(newList);
                }
                    
                File.Delete(listPath);
            }
        }
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