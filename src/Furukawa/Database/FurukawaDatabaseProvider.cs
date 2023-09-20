using Bunkum.RealmDatabase;
using Furukawa.Types;
using Realms;

namespace Furukawa.Database
{
    public class FurukawaDatabaseProvider : RealmDatabaseProvider<FurukawaDatabaseContext>
    {
        public FurukawaDatabaseProvider()
        {
        }

        public override void Warmup()
        {
            //throw new NotImplementedException();
        }

        protected override void Migrate(Migration migration, ulong oldVersion)
        {
            //throw new NotImplementedException();
        }

        protected override ulong SchemaVersion { get; }

        protected override List<Type> SchemaTypes { get; } = new()
        {
            typeof(FsrsRealmCard),
            typeof(FsrsRealmReviewLog)
        };
        protected override string Filename => "furukawa.realm";
    }   
}