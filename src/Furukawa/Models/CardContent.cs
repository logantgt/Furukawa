using System;
using Newtonsoft.Json;

namespace Furukawa.Models
{
    [JsonObject]
    public struct CardContent
    {
        public Guid CardGuid;
        public string Template;
        public string Content;
    }
}