using System.Xml.Serialization;
using Realms;

namespace Furukawa.Types;

public partial class CorpusNote : IRealmObject
{
    public CorpusNote(string sortField, string content, string template, string[] dependencies)
    {
        SortField = sortField;
        Content = content;
        Template = template;
        foreach (string dep in dependencies)
        {
            if (!String.IsNullOrWhiteSpace(dep)) Dependencies.Add(dep);
        }
    }

    [PrimaryKey]
    public string SortField { get; set; } // The "answer" of the card, just as in Anki
    public string Content { get; set; } // Plaintext path to a file which has the contents of the note
    public string Template { get; set; } // Name of the template
    public IList<string> Dependencies { get; } // List of strings which are SortFields of other notes that must be in a Review state before seeing this note
    
    internal static CorpusNote ReadFromStream(Stream stream)
    {
        XmlSerializer serializer = new(typeof(CorpusNote));
        return (CorpusNote)serializer.Deserialize(stream);
    }
}