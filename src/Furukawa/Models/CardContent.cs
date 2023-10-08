using System;
using Newtonsoft.Json;

namespace Furukawa.Models;

[JsonObject]
public struct CardContent
{
    public string Guid;
    public string Template;
    public string Content;
}