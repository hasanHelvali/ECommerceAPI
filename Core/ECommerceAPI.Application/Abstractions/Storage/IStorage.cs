using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Storage
{
    public interface IStorage
    {
        //Storage lar ile ilgili en base deki interface bu interface dir.
        //Tum service lerdeolmasını bekledigimiz yapıları buraya tasıyorum.
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName,string fileName);


    }
}
