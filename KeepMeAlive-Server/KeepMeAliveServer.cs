using KeepMeAlive.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using System.Reflection;
using Range = SemanticVersioning.Range;

namespace KeepMeAlive.Server;

//====================[ ServerModMetadata ]====================
// SPT 4 C# server mod metadata.
public record ServerModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "KeepMeAlive Server";
    public override string Author { get; init; } = "kaikiNoodles";
    public override List<string>? Contributors { get; init; } = [];
    public override List<string>? Incompatibilities { get; init; } = [];
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = [];
    public override string? Url { get; init; } = "https://github.com/thuynguyentrungdang/BringMeToLifeMod";
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
    public override string ModGuid { get; init; } = "KeepMeAliveServer";
    public override SemanticVersioning.Version Version { get; init; } = new(1, 0, 1);
    public override SemanticVersioning.Range SptVersion { get; init; } = new(">=4.0.12");
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class RevivalModServer(
    ModHelper modHelper,
    ISptLogger<RevivalModServer> logger,
    CustomStaticRouter customStaticRouter) : IOnLoad
{
    private ModConfig? _modConfig;

    public async Task OnLoad()
    {
        // Get your current assembly
        var assembly = Assembly.GetExecutingAssembly();
        var pathToMod = modHelper.GetAbsolutePathToModFolder(assembly);

        _modConfig = modHelper.GetJsonDataFromFile<ModConfig>(pathToMod, "config/config.json");

        customStaticRouter.PassConfig(_modConfig);

        logger.Info("[KeepMeAlive.Server] Config applied serverside.");

        await Task.CompletedTask;
    }

}