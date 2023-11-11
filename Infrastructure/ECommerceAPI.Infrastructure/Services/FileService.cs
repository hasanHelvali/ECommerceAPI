using ECommerceAPI.Application.Services;
using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment _webHostEnvironment)
        {
            this.webHostEnvironment = _webHostEnvironment;
        }
        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, path);
            Random rnd = new Random();
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new List<bool>();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath,file.FileName);
                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}",file);
                datas.Add((fileNewName,$"{ uploadPath}\\{ fileNewName}"));
                results.Add(result);

            }
            if (results.TrueForAll(r=>r.Equals(true)))
                return datas;
            return null;
            // todo Eger ki yukaridaki if gecerli degilse burada dosyaların sunucuda yuklenirken hata alındıgına dair uyarici bir ex olusturulup firlatilmasi gerekiyor.
    

        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                //todo log
                throw ex;
            }
        }

         async Task<string> FileRenameAsync(string path , string fileName,bool first=true)
        {
            //Asenkron bir fonksiyonun bnaslatılması bu sekilde yapılabilir.
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(path);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(path);
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo = newFileName.IndexOf("-");
                    if (indexNo==-1)
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo;
                            indexNo=newFileName.IndexOf('-', indexNo + 1);
                            if (indexNo == -1)
                            {
                                indexNo = lastIndex;
                                break;
                            }

                        }

                        int indexNo2=newFileName.IndexOf(".");
                        string fileNo=newFileName.Substring(indexNo+1,indexNo2-indexNo-1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++; 
                            newFileName = newFileName.Remove(indexNo+1, indexNo2 - (indexNo - 1))
                                .Insert(indexNo+1, _fileNo.ToString());
                        }
                        else
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}"; 
                        }
                            
                    }
                }
                if (File.Exists($"{path}\\{newFileName}"))
                    return await FileRenameAsync(path,newFileName,false);
                else
                    return newFileName;
                
                if (true)
                {

                }
            });

            return newFileName;
        }
    }
}
