using System;
using Newtonsoft.Json;

namespace Furukawa.Models;

[JsonObject]
public struct RenderedCard
{
    public string Guid;
    public int State;
    public string Content;
}