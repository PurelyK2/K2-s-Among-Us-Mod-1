
using System.Reflection;
using BepInEx.Logging;
using MiraAPI.Utilities;
using TownOfUs.Modules.Localization;

namespace TouExtensionExample.Modules;

public static class ExtensionLocale
{
    internal static ManualLogSource LocaleLogger { get; } = BepInEx.Logging.Logger.CreateLogSource("ExampleLocale");

    public static void SearchInternalLocale()
    {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var locale in TouLocale.LangList)
        {
            using var resourceStream =
                assembly.GetManifestResourceStream("TouExtensionExample.Resources.Locale." + locale.Value);
            if (resourceStream == null)
            {
                LocaleLogger.LogError($"Example Language is not added: {locale.Key.ToDisplayString()}");
                continue;
            }

            LocaleLogger.LogWarning($"Example Language is being added: {locale.Key.ToDisplayString()}");
            using StreamReader reader = new(resourceStream);
            string xmlContent = reader.ReadToEnd();

            TouLocale.TouLocalization.TryAdd((SupportedLangs)locale.Key, []);
            TouLocale.ParseXmlFile(xmlContent, (SupportedLangs)locale.Key);
        }
    }
}