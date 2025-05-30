using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace HireHive.Infrastructure.Startup
{
    public static class KeyVaultConfiguration
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            var keyVaultURL = configuration.GetSection("AzureKeyVault:KeyVaultURL").Value;
            var keyVaultClientId = configuration.GetSection("AzureKeyVault:ClientId").Value;
            var keyVaultClientSecret = configuration.GetSection("AzureKeyVault:ClientSecret").Value;
            var keyVaultDirectoryID = configuration.GetSection("AzureKeyVault:DirectoryID").Value;

            var credential = new ClientSecretCredential(keyVaultDirectoryID!.ToString(), keyVaultClientId!.ToString(), keyVaultClientSecret!.ToString());
            builder.AddAzureKeyVault(
                new Uri(keyVaultURL!),
                credential);
        }
    }
}
