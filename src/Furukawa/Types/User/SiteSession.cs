using Bunkum.Core.Authentication;
using Realms;
#pragma warning disable CS8618

namespace Furukawa.Types
{
    public class SiteSession : RealmObject, IToken<SiteUser>
    {
        [PrimaryKey] [Required] public string Id { get; init; }
    
        // Realm can't store enums, use recommended workaround
        // ReSharper disable once InconsistentNaming (can't fix due to conflict with SessionType)
        // ReSharper disable once MemberCanBePrivate.Global
        internal int _SessionType { get; set; }
        public SessionType SessionType
        {
            get => (SessionType)_SessionType;
            set => _SessionType = (int)value;
        }
        public SiteUser User { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public DateTimeOffset ExpiryDate { get; init; }
    }
}