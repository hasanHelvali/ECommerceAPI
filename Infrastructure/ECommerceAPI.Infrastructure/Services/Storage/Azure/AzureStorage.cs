using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage , IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;//ilgili azure storage acoount a baglanmamızı saglar.
        BlobContainerClient _blobContainerClient;//hedef storage da islemler yapmamızı saglar.
        //Bunlar azure storage uzerinde calisma yaparken kullanacak oldugumuz sınıflardır.
        public AzureStorage(IConfiguration configuration, BlobContainerClient blobContainerClient)
        {
            _blobServiceClient = new BlobServiceClient(configuration["Storage:Azure"]);//AppsettingsJson dosyasındaki storage altındaki azure key ine eristik.
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();

        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {


            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            //blopContainerClient a karıslık nesneyi bu sekilde elde etmis olduk.
            await _blobContainerClient.CreateIfNotExistsAsync();//Ilgili container in varlıgının kontrolunu yapıyoruz.
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName =  await FileRenameAsync(containerName, file.Name, HasFile);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            return datas;
        }
    }
}
