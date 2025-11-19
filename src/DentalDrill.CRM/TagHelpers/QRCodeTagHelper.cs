using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace DentalDrill.CRM.TagHelpers
{
    [HtmlTargetElement("qrcode")]
    public class QRCodeTagHelper : ITagHelper, ITagHelperComponent
    {
        public Int32 Order => 0;

        public Int32 Size { get; set; }

        public String Content { get; set; }

        public void Init(TagHelperContext context)
        {
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = this.Size,
                    Width = this.Size,
                    Margin = 0,
                },
                Encoder = new QRCodeWriter(),
                Renderer = new PixelDataRenderer(),
            };

            var data = writer.Write(this.Content);
            Byte[] pngBytes;
            using (var pngStream = new MemoryStream())
            {
                using (var pixelsStream = new MemoryStream(data.Pixels))
                {
                    using (var pixelsData = SKData.Create(pixelsStream))
                    {
                        using (var image = SKImage.FromPixels(new SKImageInfo(this.Size, this.Size, SKColorType.Bgra8888), pixelsData, this.Size * 4))
                        {
                            using (var pngData = image.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                pngData.SaveTo(pngStream);
                            }
                        }
                    }
                }

                pngBytes = pngStream.ToArray();
            }

            output.TagName = "img";
            output.Attributes.Clear();
            output.Attributes.Add("src", $"data:image/png;base64,{Convert.ToBase64String(pngBytes)}");
            return Task.CompletedTask;
        }
    }
}
