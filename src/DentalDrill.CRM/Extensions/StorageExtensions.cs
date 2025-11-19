using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Storage;

namespace DentalDrill.CRM.Extensions
{
    public static class StorageExtensions
    {
        public static async Task<Byte[]> GetFileBytesAsync(this IStorageHub hub, String containerName, String containerPath)
        {
            var container = hub.GetContainer(containerName);
            await using var fileStream = await container.GetFileContentAsync(containerPath);
            await using var tempStream = new MemoryStream();
            await fileStream.CopyToAsync(tempStream);
            return tempStream.ToArray();
        }

        public static async Task<Byte[]> TryGetFileBytesAsync(this IStorageHub hub, String containerName, String containerPath)
        {
            try
            {
                var container = hub.GetContainer(containerName);
                await using var fileStream = await container.GetFileContentAsync(containerPath);
                await using var tempStream = new MemoryStream();
                await fileStream.CopyToAsync(tempStream);
                return tempStream.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<String> GetFileTextAsync(this IStorageHub hub, String containerName, String containerPath, Encoding encoding)
        {
            var container = hub.GetContainer(containerName);
            await using var fileStream = await container.GetFileContentAsync(containerPath);
            using var streamReader = new StreamReader(fileStream, encoding, false, 1024, true);
            return await streamReader.ReadToEndAsync();
        }

        public static async Task<String> TryGetFileTextAsync(this IStorageHub hub, String containerName, String containerPath, Encoding encoding)
        {
            try
            {
                var container = hub.GetContainer(containerName);
                await using var fileStream = await container.GetFileContentAsync(containerPath);
                using var streamReader = new StreamReader(fileStream, encoding, false, 1024, true);
                return await streamReader.ReadToEndAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
