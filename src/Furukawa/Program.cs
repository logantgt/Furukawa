using Bunkum.Core;
using Furukawa;

BunkumConsole.AllocateConsole();

FurukawaServer server = new();
server.Initialize();

server.Start();
await Task.Delay(-1);