using System;
using System.Reflection;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Services;
using FSRSharp;
using NotEnoughLogs;

namespace Furukawa.Services;

/// <summary>
/// A service that provides an instance of the Fsrs algorithm to endpoints.
/// </summary>
public class FsrsService : Service
{
    private FsrsAlgorithm _fsrs = new FsrsAlgorithm();
    
    internal FsrsService(LoggerContainer<BunkumContext> logger) : base(logger) { }
    
    public override object? AddParameterToEndpoint(ListenerContext context, ParameterInfo paramInfo, Lazy<IDatabaseContext> database)
    {
        this.Logger.LogDebug(BunkumContext.Service, $"FsrsService is attempting to pass something in for `{paramInfo.ParameterType.Name} {paramInfo.Name}`");
        if (paramInfo.ParameterType == typeof(FsrsAlgorithm))
        {
            this.Logger.LogDebug(BunkumContext.Service, "Matched! Passing the time in.");
            return _fsrs;
        }

        this.Logger.LogDebug(BunkumContext.Service, "No dice. Won't pass anything in for this parameter.");
        return base.AddParameterToEndpoint(context, paramInfo, database);
    }
}