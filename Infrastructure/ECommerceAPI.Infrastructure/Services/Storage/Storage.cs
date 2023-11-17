using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        /*Mimaride ortak olan yapılar IStorage a konulup oradan implemente edilirdi. İlgili yapılarda o anki storage yapılanmasına gore gerekli concrete
         lerde icleri dolduruluyordu. 
        Lakin buradaki yapı herhangi bir storage yapılanmasında ici doldurulacak bir yapı degildir. Her storage yapısı icin ortak olarak calısacak 
        bir yapıdır. Bu sebeple interface e koyup her implemente isleminde ilgili storage in concrete inde bunu doldurmak yerine , buradan, ilgili concrete lerin 
        kullanmasını saglamak istiyoruz. Kendimizi tekrar etmemmek adına bunu yapıyoruz.*/



        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName,HasFile hasFileMethod, bool first = true)
            //Sadece kalıtımsal olarak kullanılması gerektiginden dolayı protected olarak isaretlendi.
        {
            //Asenkron bir fonksiyonun bnaslatılması bu sekilde yapılabilir.
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo = newFileName.IndexOf("-");
                    if (indexNo == -1)
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo;
                            indexNo = newFileName.IndexOf('-', indexNo + 1);
                            if (indexNo == -1)
                            {
                                indexNo = lastIndex;
                                break;
                            }

                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo + 1, indexNo2 - indexNo - 1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo + 1, indexNo2 - (indexNo - 1))
                                .Insert(indexNo + 1, _fileNo.ToString());
                        }
                        else
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                        }

                    }
                }
                //if (File.Exists($"{path}\\{newFileName}"))
                //    return await FileRenameAsync(path, newFileName, false);
                //else
                //    return newFileName;

                //Artık burada hangi concrete ile calısılıyorsa ona gore bir dosyanın varlıgının kontrol edilmesi gerekiyor.
                if (hasFileMethod(pathOrContainerName, newFileName))
                    return await FileRenameAsync(pathOrContainerName, newFileName,hasFileMethod, false); 
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
