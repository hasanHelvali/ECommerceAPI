using ECommerceAPI.Infrastructure.Operations;
namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService
    {
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
