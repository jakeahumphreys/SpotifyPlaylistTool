﻿using System.Security.Cryptography;
using System.Text;
using JSpotifyClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyPlaylistTool.Settings;
using SpotifyPlaylistTool.SpotifyApi;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SpotifyPlaylistTool;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        var appSettings = LoadSettingsFile();

        builder.Services.AddSingleton(appSettings);
        builder.Services.AddSingleton<ISpotifyClient>(new SpotifyClient(appSettings.SpotifyClientId, appSettings.SpotifyClientSecret));
        builder.Services.AddSingleton<ISpotifyClientService, SpotifyClientService>();
        
        return builder.Build();
    }

    private static AppSettings LoadSettingsFile()
    {
        var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Spotify Playlist Tool");

        if (!Directory.Exists(appDirectory))
            Directory.CreateDirectory(appDirectory);

        var settingsFilePath = Path.Combine(appDirectory, "appsettings.json");
        if (!File.Exists(settingsFilePath))
        {
            var appSettings = new AppSettings();
            var appSettingsJsonString = JsonSerializer.Serialize(appSettings);
            File.WriteAllText(settingsFilePath, appSettingsJsonString);
            return appSettings;
        }

        var json = File.ReadAllText(settingsFilePath);
        var existingAppSettings = JsonConvert.DeserializeObject<AppSettings>(json);
        var jsonProperties = JObject.Parse(json).Properties().ToList();

        var sha256 = SHA256.Create();
        var fileHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        var objectHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(existingAppSettings)));

        if (fileHash.SequenceEqual(objectHash))
            return existingAppSettings;

        foreach (var property in typeof(AppSettings).GetProperties())
        {
            var jsonProperty = jsonProperties.FirstOrDefault(x => x.Name.Equals(property.Name));
            
            if(jsonProperty == null)
                property.SetValue(existingAppSettings, null);
            else
                property.SetValue(existingAppSettings, jsonProperty.Value.ToObject(property.PropertyType));
        }

        var updatedJson = new JObject(typeof(AppSettings).GetProperties().Select(x => new JProperty(x.Name, x.GetValue(existingAppSettings))));
        File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(updatedJson));
        
        return existingAppSettings;
    }
}