//====================[ Imports ]====================
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using KeepMeAlive.Server.Models.Revival;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Utils;

namespace KeepMeAlive.Server.Services;

//====================[ RevivalConfigService ]====================
[Injectable(InjectionType.Singleton)]
public class RevivalConfigService(ModHelper modHelper, JsonUtil jsonUtil)
{
    //====================[ State ]====================
    public RevivalServerConfig Config { get; private set; } = new();

    public string ModPath => modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
    public string ConfigPath => Path.Combine(ModPath, "config.json");

    //====================[ Lifecycle ]====================
    public async Task OnPreLoadAsync()
    {
        Config = await LoadConfigAsync(ConfigPath);
        NormalizeConfig();

        // Ensure defaults are persisted when new fields are added.
        await PersistConfigAsync(ConfigPath);
    }

    //====================[ Snapshot ]====================
    public RevivalRuntimeConfigSnapshot GetRuntimeSnapshot()
    {
        NormalizeConfig();

        string normalizedJson = jsonUtil.Serialize(Config, false) ?? string.Empty;
        string hash = ComputeSha256Hex(normalizedJson);

        return new RevivalRuntimeConfigSnapshot
        {
            SchemaVersion = Config.SchemaVersion,
            ConfigHash = hash,
            GeneratedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Config = Config
        };
    }

    //====================[ Internal Helpers ]====================
    private async Task<RevivalServerConfig> LoadConfigAsync(string path)
    {
        if (File.Exists(path))
        {
            return await jsonUtil.DeserializeFromFileAsync<RevivalServerConfig>(path) ?? new RevivalServerConfig();
        }

        return new RevivalServerConfig();
    }

    private async Task PersistConfigAsync(string path)
    {
        await File.WriteAllTextAsync(path, jsonUtil.Serialize(Config, true));
    }

    private void NormalizeConfig()
    {
        Config ??= new RevivalServerConfig();
        Config.Normalize();
        Config.SchemaVersion = RevivalServerConfig.CurrentSchemaVersion;
    }

    private static string ComputeSha256Hex(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        var bytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes);
    }
}
