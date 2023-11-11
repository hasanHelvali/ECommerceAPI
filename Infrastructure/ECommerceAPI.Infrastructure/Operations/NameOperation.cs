using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Operations
{
    public class NameOperation
    {
        public static string CharacterRegulatory(string name)
            => name.Replace("\"", "")
                .Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("+", "")
                .Replace("%", "")
                .Replace("&", "")
                .Replace("/", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace("_", "")
                .Replace("@", "")
                .Replace("₺", "")
                .Replace("€", "")
                .Replace("~", "")
                .Replace("´", "")
                .Replace("`", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("Ö", "")
                .Replace("ö", "")
                .Replace("Ü", "")
                .Replace("ü", "")
                .Replace("İ", "")
                .Replace("ı", "")
                .Replace("Ğ", "")
                .Replace("ğ", "")
                .Replace("Ç", "")
                .Replace("ç", "")
                .Replace("Ş", "")
                .Replace("ş", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "")
                .Replace("*", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("#", "")
                .Replace("''", "");
    }
}
