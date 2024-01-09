using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.CloudStorage.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CloudStorage
{
    public static class CloudStorageExtensions
    {
        public static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = new GoogleCloudStorageOptions();
            Configuration.GetSection(nameof(GoogleCloudStorageOptions)).Bind(options);

            services.Configure<GoogleCloudStorageOptions>(Configuration.GetSection(nameof(GoogleCloudStorageOptions)));

            var googleCredential = GoogleCredential.FromFile(options.GoogleCredentialFilePath);
            var storageClient = StorageClient.Create(googleCredential);
            
            services.AddSingleton(storageClient);
            services.AddSingleton<ICloudStorage, GoogleCloudStorage>();

            return services;
        }
    }
}
