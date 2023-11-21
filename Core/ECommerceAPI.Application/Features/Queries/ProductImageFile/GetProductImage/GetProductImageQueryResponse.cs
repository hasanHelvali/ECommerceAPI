using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImage
{
    public class GetProductImageQueryResponse
    {
        public Guid ID { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
