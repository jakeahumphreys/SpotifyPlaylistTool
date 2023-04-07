using System.Text.Json;
using JSpotifyClient;
using SpotifyPlaylistTool.Settings;

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

        var appSettings = LoadOrCreateSettings();

        builder.Services.AddSingleton(appSettings);
        builder.Services.AddSingleton<ISpotifyClient>(new SpotifyClient(appSettings.SpotifyClientId, appSettings.SpotifyClientSecret));
        
        return builder.Build();
    }

    private static AppSettings LoadOrCreateSettings()
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

        var settingsFileJson = File.ReadAllText(settingsFilePath);

        return JsonSerializer.Deserialize<AppSettings>(settingsFileJson);
    }
}