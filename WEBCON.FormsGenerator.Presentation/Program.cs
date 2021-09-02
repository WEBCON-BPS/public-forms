using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WEBCON.FormsGenerator.Presentation
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        private static string KeyVaultEndpoint => "https://webconformsgenerator-kv.vault.azure.net/";
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
                {
                    logging.AddAzureWebAppDiagnostics();
                }
                logging.AddNLog(context.Configuration.GetSection("Logging"));
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((ctx, builder) =>
               {
                   if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
                   {
                       var keyVaultEndpoint = KeyVaultEndpoint;
                       if (!string.IsNullOrEmpty(keyVaultEndpoint))
                       {
                           var azureServiceTokenProvider = new AzureServiceTokenProvider();
                           var keyVaultClient = new KeyVaultClient(
                               new KeyVaultClient.AuthenticationCallback(
                                   azureServiceTokenProvider.KeyVaultTokenCallback));
                           builder.AddAzureKeyVault(
                               keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                       }
                   }
               }
            );
    }
}
