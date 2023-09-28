using System.Globalization;
using Bunkum.RealmDatabase;
using Furukawa.Types;

namespace Furukawa.Database
{
    public class FurukawaDatabaseContext : RealmDatabaseContext
    {
        public FsrsRealmCard QueryNextCard()
        {
            // TODO: properly query database for set of next due cards and return next due card (first card to become due by timestamp)
            IQueryable<FsrsRealmCard> dueCardList = _realm.All<FsrsRealmCard>();
            return dueCardList.First();
        }

        public CorpusNote QueryNote(string note)
        {
            return _realm.Find<CorpusNote>(note + "\r");
        }

        public void WriteFsrsRealmReviewLog(FsrsRealmReviewLog reviewLog)
        {
            _realm.Write(() =>
            {
                _realm.Add(reviewLog);
            });
        }
        
        public void WriteFsrsRealmCard(FsrsRealmCard card)
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
        
        public void AddSomeCorpusListsTest(CorpusList list)
        {
            _realm.Write(() =>
            {
                _realm.Add(list, true);
            });
        }
    }   
}