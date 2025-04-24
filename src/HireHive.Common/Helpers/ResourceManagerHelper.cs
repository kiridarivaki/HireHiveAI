using System.Reflection;
using System.Resources;

namespace HireHive.Common.Helpers;

public static class ResourceManagerHelper
{
    public static string GetString(string fileName, string key, string? assemblyName = null)
    {
        var resourceManager = GetResourceManager(fileName, assemblyName);
        return resourceManager.GetString(key)!;
    }

    private static ResourceManager GetResourceManager(string fileName, string? assemblyName = null)
    {
        assemblyName ??= Assembly.GetEntryAssembly()!.GetName().Name!;
        return new ResourceManager(
            baseName: assemblyName + $".Resources.Properties.{fileName}",
            assembly: Assembly.Load(assemblyName));
    }
}
