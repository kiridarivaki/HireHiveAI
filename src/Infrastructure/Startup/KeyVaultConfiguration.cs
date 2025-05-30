using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace HireHive.Infrastructure.Startup
{
    public static class KeyVaultConfiguration
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            var keyVaultURL = configuration.GetValue<string>("AzureKeyVault:KeyVaultURL");
            var keyVaultClientId = configuration.GetValue<string>("AzureKeyVault:ClientId");
            var keyVaultClientSecret = configuration.GetValue<string>("AzureKeyVault:ClientSecret");
            var keyVaultDirectoryID = configuration.GetValue<string>("AzureKeyVault:DirectoryID");

            var credential = new ClientSecretCredential(keyVaultDirectoryID!.ToString(), keyVaultClientId!.ToString(), keyVaultClientSecret!.ToString());
            builder.AddAzureKeyVault(
                new Uri(keyVaultURL!),
                credential);
        }
    }
}
