using Realms;

namespace Furukawa.Types
{
    public partial class CorpusTemplate : IRealmObject
    {
        [PrimaryKey]
        public string Type { get; set; } // The main identifier of the template
        public string Path { get; set; } // Plaintext path to an HTML file which has the template
    }
}