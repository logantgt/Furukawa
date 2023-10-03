using System.Reflection;
using Bunkum.Core.Authentication;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using Furukawa.Authentication;
using Furukawa.Configuration;
using Furukawa.Database;
using Furukawa.Middlewares;
using Furukawa.Services;
using Furukawa.Types;

namespace Furukawa
{
    public class FurukawaServer
    {
        protected readonly BunkumHttpServer ServerInstance;
        protected readonly FurukawaDatabaseProvider DatabaseProvider;
        protected readonly IDataStore DataStore;
    
        public FurukawaServer(BunkumHttpListener? listener = null,
            FurukawaDatabaseProvider? databaseProvider = null,
            IAuthenticationProvider<SiteSession>? authProvider = null,
            IDataStore? dataStore = null)
        {
            databaseProvider ??= new FurukawaDatabaseProvider();
            dataStore ??= new FileSystemDataStore();
            //authProvider ??= new SessionProvider();
            
            DatabaseProvider = databaseProvider;
    
            DataStore = dataStore;
            
            ServerInstance = listener == null ? new BunkumHttpServer() : new BunkumHttpServer(listener);
            
            ServerInstance.UseDatabaseProvider(databaseProvider);
            ServerInstance.AddAuthenticationService(authProvider, true);
    
            ServerInstance.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        public Task StartAndBlockAsync()
        {
            return ServerInstance.StartAndBlockAsync();
        }
        
        public void Start()
        {
            ServerInstance.Start();
        }
    
        public void Initialize()
        {
            DatabaseProvider.Initialize();
            
            SetUpConfiguration();
            SetUpServices();
            SetUpMiddlewares();
        }
    
        protected virtual void SetUpConfiguration()
        {
            ServerInstance.UseJsonConfig<ExampleConfiguration>("gameServer.json");
        }
        
        protected virtual void SetUpServices()
        {
            ServerInstance.AddRateLimitService(new RateLimitSettings(30, 40, 0, "global"));
            ServerInstance.AddService<FsrsService>();
        }
    
        protected virtual void SetUpMiddlewares()
        {
            ServerInstance.AddMiddleware<WebsiteMiddleware>();
        }
    }
}