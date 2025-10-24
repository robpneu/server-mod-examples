using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;

namespace _26RouterHook;

/// <summary>
/// This is the replacement for the former package.json data. This is required for all mods.
///
/// This is where we define all the metadata associated with this mod.
/// You don't have to do anything with it, other than fill it out.
/// All properties must be overriden, properties you don't use may be left null.
/// It is read by the mod loader when this mod is loaded.
/// </summary>
public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.sp-tarkov.examples.routerhook";
    public override string Name { get; init; } = "RouterHookExample";
    public override string Author { get; init; } = "SPTarkov";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    
    
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; }
    public override string? License { get; init; } = "MIT";
}

[Injectable]
public class RouterHook : StaticRouter
{
    private static ISptLogger<RouterHook> _logger;

    public RouterHook(
        JsonUtil jsonUtil,
        ISptLogger<RouterHook> logger) : base(
        jsonUtil,
        // Add an array of routes to which we want to listen
        GetRouteListeners()
    )
    {
        _logger = logger;
    }

    private static List<RouteAction> GetRouteListeners()
    {
        return
        [
            new RouteAction(
                "/client/game/start",
                async (
                    url,
                    info,
                    sessionId,
                    output
                ) => await HandleRouteSync(url, sessionId, output)
            ),
            new RouteAction(
                "/client/game/logout",
                async (
                    url,
                    info,
                    sessionId,
                    output
                ) => await HandleRouteAsync(url, sessionId, output)
            )
        ];
    }

    /**
     * Example of an asynchronous route listener
     * This WILL NOT block the original route action while executing
     */
    private static ValueTask<string> HandleRouteAsync(string url, MongoId sessionId, String? output)
    {
        // Spin off a new task to run asynchronously
        _ = Task.Run(async () => 
        {
            // Your mod's asynchronous code goes here
            
            // Simulates some asynchronous work (DELETE THIS IN PRODUCTION MODS)
            _logger.Info($"Async work started from route {url} for session {sessionId}");
            // await Task.Delay(10000);
            Thread.Sleep(10000);
            _logger.Info($"Async work completed from route {url} for session {sessionId}");
        });

        // Unless you want to modify what the route returned, return the output unmodified.
        return new ValueTask<string>(output);
    }

    /**
     * Example of a synchronous route listener
     * This WILL block the original route action while executing
     */
    private static ValueTask<string> HandleRouteSync(string url, MongoId sessionId, String? output)
    {
        // Your mod's synchronous code goes here
        
        // Simulates some synchronous work (DELETE THIS IN PRODUCTION MODS)
        _logger.Info($"Synchronous work started from route {url} for session {sessionId}");
        Thread.Sleep(10000);
        _logger.Info($"Synchronous work completed from route {url} for session {sessionId}");

        // Unless you want to modify what the route returned, return the output unmodified.
        return new ValueTask<string>(output);
    }
    
}