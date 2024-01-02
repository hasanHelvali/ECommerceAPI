using ECommerceAPI.Application.Abstractions.Services;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public QRCodeService()
        {
        }

        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQRCode = new PngByteQRCode(qRCodeData);
            byte[] byteGraphic = pngByteQRCode.GetGraphic(10, new Byte[] { 84, 99, 71 }, new Byte[] { 240, 240, 240 });
            return byteGraphic;
        }
    }
}
