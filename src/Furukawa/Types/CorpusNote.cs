using Realms;

namespace Furukawa.Types
{
    public partial class CorpusNote : IRealmObject
    {
        public CorpusNote(string sortField, string content, string[] dependencies)
        {
            SortField = sortField;
            Content = content;
            foreach (string dep in dependencies)
            {
                if (String.IsNullOrWhiteSpace(dep)) continue;
                Dependencies.Add(dep);
            }
        }

        [PrimaryKey]
        public string SortField { get; set; } // The "answer" of the card, just as in Anki
        public string Content { get; set; } // Plaintext path to an HTML file which has the contents of the note
        public IList<string> Dependencies { get; } // List of strings which are SortFields of other notes that must be in a Review state before seeing this note
    }
}