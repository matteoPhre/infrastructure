using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CloudStorage.Google
{
    public class GoogleCloudStorage : ICloudStorage
    {
        private readonly StorageClient _storageClient;
        private readonly GoogleCloudStorageOptions _googleCloudStorageOptions;

        public GoogleCloudStorage(StorageClient storageClient, IOptions<GoogleCloudStorageOptions> googleCloudStorageOptions)
        {
            _storageClient = storageClient;
            _googleCloudStorageOptions = googleCloudStorageOptions.Value;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_googleCloudStorageOptions.GoogleCloudStorageBucketName, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> GetFileAsync(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var googleObject = await _storageClient.GetObjectAsync(_googleCloudStorageOptions.GoogleCloudStorageBucketName, fileName, null, cancellationToken);
                return googleObject != null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var folder = string.Concat(Guid.NewGuid().ToString(), "/");

                    var dataObj = await _storageClient.UploadObjectAsync(_googleCloudStorageOptions.GoogleCloudStorageBucketName,
                        string.Concat(folder, fileName),
                        null,
                        memoryStream);
                    return dataObj.MediaLink;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
