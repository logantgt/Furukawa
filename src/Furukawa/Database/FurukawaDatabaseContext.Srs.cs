using System.Globalization;
using Bunkum.RealmDatabase;
using Furukawa.Types;

namespace Furukawa.Database;

public partial class FurukawaDatabaseContext : RealmDatabaseContext
{
    public SerializableCard QueryNextCard()
    {
        // TODO: properly query database for set of next due cards and return next due card (first card to become due by timestamp)
        IQueryable<SerializableCard> dueCardList = _realm.All<SerializableCard>();
        return dueCardList.First();
    }

    public CorpusNote QueryNote(string note)
    {
        return _realm.Find<CorpusNote>(note);
    }

    public SerializableCard GetFsrsCardByGuid(Guid guid)
    {
        return _realm.Find<SerializableCard>(guid);
    }

    public void WriteFsrsRealmReviewLog(SerializableReviewLog reviewLog)
    {
        _realm.Write(() =>
        {
            _realm.Add(reviewLog);
        });
    }
        
    public void WriteFsrsRealmCard(SerializableCard card)
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

    public string ReadCorpusTemplate(string key)
    {
        return _realm.Find<CorpusTemplate>(key).Content;
    }

    public void WriteCorpusTemplate(CorpusTemplate template)
    {
        _realm.Write(() =>
        {
            _realm.Add(template, true);
        });
    }

    private static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}