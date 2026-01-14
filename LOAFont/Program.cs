using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace LOAFont;

internal class Program
{
    private const string FontFile = "font.lpk";
    private const string GameId = "1599340";

    private static void Main()
    {
        string localFontFile = Path.Combine(AppContext.BaseDirectory, FontFile);
        if (!File.Exists(localFontFile))
        {
            Notify($"Custom font was not found. \nPlace it next to LOAFont.exe and make sure it's named {FontFile}.");
            return;
        }

        if (!TryGetSteamInstallationPath(out string? steamPath))
        {
            Notify("Steam is not installed.");
            return;
        }

        if (!TryGetGameInstallationPath(steamPath, out string? installPath))
        {
            Notify("Lost Ark is not installed.");
            return;
        }

        ReplaceFont(localFontFile, installPath);

        var args = Environment.GetCommandLineArgs();

        string gameExe = args[1]; // The second argument is the game executable path.
        string[] gameArgs = args[2..]; // The rest are the game arguments.

        // Launch the game.
        Process.Start(new ProcessStartInfo
        {
            FileName = gameExe,
            Arguments = string.Join(" ", gameArgs),
            UseShellExecute = true
        });
    }

    private static bool TryGetSteamInstallationPath([NotNullWhen(true)] out string? steamPath)
    {
        // Gets the Steam installation path from the registry.
        steamPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string;
        return steamPath is not null;
    }

    private static bool TryGetGameInstallationPath(string steamPath, [NotNullWhen(true)] out string? path)
    {
        // Deserializes Steam's library file, which contains all Steam library paths and the game IDs installed in each one.
        var library = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamPath, "steamapps", "libraryfolders.vdf")));

        // Iterates through all Steam library paths until it finds the one containing the Lost Ark Game Id.
        foreach (var property in library.Value.Children<VProperty>())
        {
            if (property.Value["apps"] is VObject apps && apps.ContainsKey(GameId))
            {
                path = property.Value["path"]?.Value<string>();
                return path is not null;
            }
        }

        path = null;
        return false;
    }

    private static void ReplaceFont(string localFontFile, string installPath)
    {
        string efGamePath = Path.Combine(installPath, "steamapps", "common", "Lost Ark", "EFGame");
        string gameFontFile = Path.Combine(efGamePath, FontFile);

        // If game's font file doesn't exist, copy the local font directly.
        if (!File.Exists(gameFontFile))
        {
            File.Copy(localFontFile, gameFontFile);

            Notify("Successfully replaced game font.");
            return;
        }

        string gameFileSHA256 = ComputeSHA256(gameFontFile);
        string localFileSHA256 = ComputeSHA256(localFontFile);

        // Compare the file hashes. If they are different, replace the game font.
        if (!gameFileSHA256.Equals(localFileSHA256))
        {
            // Create a backup file of the original font.
            string backup = Path.Combine(efGamePath, FontFile + ".bak");
            File.Copy(gameFontFile, backup, true);

            File.Copy(localFontFile, gameFontFile, true);

            Notify("Successfully replaced game font.");
        }
    }

    private static string ComputeSHA256(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha256 = SHA256.Create();

        byte[] hash = sha256.ComputeHash(stream);

        return Convert.ToHexString(hash);
    }

    private static void Notify(string message)
    {
        new ToastContentBuilder()
            .AddText(message)
            .Show();
    }
}