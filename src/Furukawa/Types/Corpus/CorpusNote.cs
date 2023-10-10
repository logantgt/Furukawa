using System.Xml.Serialization;
using Realms;

namespace Furukawa.Types;

public partial class CorpusNote : IRealmObject
{
    public CorpusNote() { }

    public CorpusNote(string sortField, Dictionary<string, string> content, string template, string[] dependencies)
    {
        SortField = sortField;
        foreach (KeyValuePair<string, string> entry in content)
        {
            Content.Add(entry);
        }
        Template = template;
        foreach (string dep in dependencies)
        {
            if (!String.IsNullOrWhiteSpace(dep)) Dependencies.Add(dep);
        }
    }

    [PrimaryKey]
    public string SortField { get; set; } // The "answer" of the card, just as in Anki
    public IDictionary<string, string> Content { get; } // Dictionary which holds the values of the card
    public string Template { get; set; } // Name of the template
    public IList<string> Dependencies { get; } // List of strings which are SortFields of other notes that must be in a Review state before seeing this note
    
    internal static CorpusNote ReadFromStream(Stream stream)
    {
        XmlSerializer serializer = new(typeof(CorpusNote));
        return (CorpusNote)serializer.Deserialize(stream);
    }
}