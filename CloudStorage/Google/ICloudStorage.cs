using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CloudStorage.Google
{
    public interface ICloudStorage
    {
        Task<bool> GetFileAsync(string fileName, CancellationToken cancellationToken);
        Task<string> UploadFileAsync(IFormFile file, string fileName);
        Task DeleteFileAsync(string fileName);

    }
}
