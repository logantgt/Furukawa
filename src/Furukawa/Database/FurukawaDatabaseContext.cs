using System.Globalization;
using Bunkum.RealmDatabase;
using Furukawa.Types;

namespace Furukawa.Database;

public partial class FurukawaDatabaseContext : RealmDatabaseContext
{
    public FsrsCard QueryNextCard()
    {
        // TODO: properly query database for set of next due cards and return next due card (first card to become due by timestamp)
        IQueryable<FsrsCard> dueCardList = _realm.All<FsrsCard>();
        return dueCardList.First();
    }

    public CorpusNote QueryNote(string note)
    {
        return _realm.Find<CorpusNote>(note);
    }

    public void WriteFsrsRealmReviewLog(FsrsReviewLog reviewLog)
    {
        _realm.Write(() =>
        {
            _realm.Add(reviewLog);
        });
    }
        
    public void WriteFsrsRealmCard(FsrsCard card)
    {
        _realm.Write(() =>
        {
            _realm.Add(card, true);
        });
    }

    public void AddSomeCorpusNotesTest(CorpusNote note)
    {
        _realm.Write(() =>
        {
            _realm.Add(note, true);
        });
    }
        
    public void AddSomeCorpusListsTest(CorpusStat list)
    {
        _realm.Write(() =>
        {
            _realm.Add(list, true);
        });
    }

    public string ReadCorpusTemplate(string key)
    {
        return _realm.Find<CorpusTemplate>(key).Content;
    }

    private static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}