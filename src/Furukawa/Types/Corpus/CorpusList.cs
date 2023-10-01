using Realms;

namespace Furukawa.Types
{
    public partial class CorpusList : IRealmObject
    {
        public string SortField { get; set; } // A SortField value to be associated with this List
        public string Deck { get; set; } // The name of the deck which includes the note
        public int Order { get; set; } // The order of the note in the deck (starting from 0)
        public int Occurances { get; set; } // The number of times the SortField appeared in the Deck
    }
}