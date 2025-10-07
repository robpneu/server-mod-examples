using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;

namespace _10CustomRoute;

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
    public override string ModGuid { get; init; } = "com.sp-tarkov.examples.customroute";
    public override string Name { get; init; } = "CustomStaticRouterExample";
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
public class CustomStaticRouter : StaticRouter
{
    private static HttpResponseUtil _httpResponseUtil;

    public CustomStaticRouter(
        JsonUtil jsonUtil,
        HttpResponseUtil httpResponseUtil) : base(
        jsonUtil,
        // Add an array of routes we want to add
        GetCustomRoutes()
    )
    {
        _httpResponseUtil = httpResponseUtil;
    }

    private static List<RouteAction> GetCustomRoutes()
    {
        return
        [
            new RouteAction(
                "/example/route/static",
                async (
                    url,
                    info,
                    sessionId,
                    output
                ) => await HandleRoute(url, info as ExampleStaticRequestData, sessionId)
            )
        ];
    }

    private static ValueTask<string> HandleRoute(string url, ExampleStaticRequestData info, MongoId sessionId)
    {
        // Your mods code goes here

        return new ValueTask<string>(_httpResponseUtil.NullResponse());
    }
}
public class ExampleStaticRequestData : IRequestData
{
}