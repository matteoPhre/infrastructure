using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CloudStorage.Google
{
    public class GoogleCloudStorageOptions
    {
        public string GoogleCredentialFilePath { get; set; }
        public string GoogleCloudStorageBucketName { get; set; }
    }
}
