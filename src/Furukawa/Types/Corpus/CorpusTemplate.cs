using System.Xml.Serialization;
using Realms;

namespace Furukawa.Types;

public partial class CorpusTemplate : IRealmObject
{
    [PrimaryKey]
    public string Name { get; set; } // The main identifier of the template
    public string Content { get; set; } // HTML of the template
    
    internal static CorpusTemplate ReadFromStream(Stream stream)
    {
        XmlSerializer serializer = new(typeof(CorpusTemplate));
        return (CorpusTemplate)serializer.Deserialize(stream);
    }
}