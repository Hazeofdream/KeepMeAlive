//====================[ Imports ]====================
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using KeepMeAlive.Server.Models.Revival;
using KeepMeAlive.Server.Services;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers.Http;
using SPTarkov.Server.Core.Utils;

namespace KeepMeAlive.Server.Http;

//====================[ RevivalStateHttpListener ]====================
[Injectable(TypePriority = 0)]
public class RevivalStateHttpListener(
    RevivalStateService stateService,
    RevivalConfigService configService,
    HttpResponseUtil httpResponseUtil,
    ISptLogger<RevivalStateHttpListener> logger) : IHttpListener
{
    //====================[ Constants ]====================
    private const string BasePath = "/kaikinoodles/keepmealive/state/";

    //====================[ Listener API ]====================
    public bool CanHandle(MongoId sessionId, HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        return context.Request.Method == "POST" && path.StartsWith(BasePath, StringComparison.OrdinalIgnoreCase);
    }

    public async Task Handle(MongoId sessionId, HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        var body = await ReadBodyAsync(context.Request);
        var info = ParseJson(body);

        if (info == null)
        {
            logger.Warning($"[KeepMeAlive.Server] Invalid JSON payload for route {path}. Body length={body.Length}");
        }

        string json;
        try
        {
            var response = Dispatch(path, info);
            json = httpResponseUtil.NoBody(response);
        }
        catch (Exception ex)
        {
            logger.Warning($"[KeepMeAlive.Server] Authority route failed for {path}: {ex.Message}");
            json = JsonSerializer.Serialize(new RevivalAuthorityResponse
            {
                Success = false,
                DenialCode = RevivalDeniedCode.ServerError,
                Reason = "Server error"
            });
        }

        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }

    //====================[ Request Parsing ]====================
    private async Task<string> ReadBodyAsync(HttpRequest request)
    {
        if (request.ContentLength is null or 0)
            return "{}";

        request.EnableBuffering();
        request.Body.Position = 0;

        // SPT/EFT request bodies are compressed unless requestcompressed=0.
        bool isCompressed = !request.Headers.TryGetValue("requestcompressed", out var cv) || cv != "0";
        string body = string.Empty;

        if (isCompressed)
        {
            try
            {
                await using var zlibStream = new ZLibStream(request.Body, CompressionMode.Decompress, leaveOpen: true);
                using var zlibReader = new StreamReader(zlibStream, Encoding.UTF8);
                body = await zlibReader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                // If decompression fails, fall back to plain read to avoid hard-failing the route.
                logger.Warning($"[KeepMeAlive.Server] Decompression failed for authority request: {ex.Message}");
                request.Body.Position = 0;
            }
        }

        if (string.IsNullOrEmpty(body))
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            body = await reader.ReadToEndAsync();
        }

        request.Body.Position = 0;
        return string.IsNullOrEmpty(body) ? "{}" : body;
    }

    private static Dictionary<string, JsonElement>? ParseJson(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return null;

            var d = new Dictionary<string, JsonElement>();
            foreach (var p in root.EnumerateObject())
                d[p.Name] = p.Value.Clone();
            return d;
        }
        catch
        {
            return null;
        }
    }

    private static string GetStr(Dictionary<string, JsonElement>? d, string key)
    {
        if (d == null || !d.TryGetValue(key, out var v))
            return string.Empty;
        return v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : v.ToString();
    }

    //====================[ Route Dispatch ]====================
    private object Dispatch(string path, Dictionary<string, JsonElement>? info)
    {
        if (path.EndsWith("begin-critical", StringComparison.OrdinalIgnoreCase))
        {
            var state = stateService.SetBleedingOut(GetStr(info, "PlayerId"));
            return new RevivalAuthorityResponse { Success = true, State = state };
        }

        if (path.EndsWith("request-revive-start", StringComparison.OrdinalIgnoreCase))
        {
            return stateService.TryStartRevive(
                GetStr(info, "PlayerId"),
                GetStr(info, "ReviverId"),
                GetStr(info, "Source"));
        }

        if (path.EndsWith("complete-revive", StringComparison.OrdinalIgnoreCase))
        {
            return stateService.TryCompleteRevive(GetStr(info, "PlayerId"), GetStr(info, "ReviverId"));
        }

        if (path.EndsWith("end-invulnerability", StringComparison.OrdinalIgnoreCase))
        {
            var state = stateService.MarkCooldown(GetStr(info, "PlayerId"));
            return new RevivalAuthorityResponse { Success = true, State = state };
        }

        if (path.EndsWith("reset", StringComparison.OrdinalIgnoreCase))
        {
            var state = stateService.Reset(GetStr(info, "PlayerId"));
            return new RevivalAuthorityResponse { Success = true, State = state };
        }

        if (path.EndsWith("get", StringComparison.OrdinalIgnoreCase))
        {
            var state = stateService.GetOrCreate(GetStr(info, "PlayerId"));
            return new RevivalAuthorityResponse { Success = true, State = state };
        }

        if (path.EndsWith("get-runtime-config", StringComparison.OrdinalIgnoreCase))
        {
            return configService.GetRuntimeSnapshot();
        }

        throw new InvalidOperationException($"Unknown route: {path}");
    }
}
