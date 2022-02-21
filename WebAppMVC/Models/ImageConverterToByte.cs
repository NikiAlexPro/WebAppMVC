using System.Drawing;
namespace WebAppMVC.Models
{
    public static class ImageConverterToByte
    {
        static Image image;
        

        public static string ContentType(IFormFile formFile)
        {
            return formFile.ContentType;
        }

        /// <param name="formFile"></param>
        public static byte[] ConvertToByte(IFormFile formFile)
        {
            
            switch (formFile.ContentType)
            {
                case "image/jpeg":
                    image = new Bitmap(formFile.OpenReadStream());
                    using(MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                    break;
                case "image/jpg":
                    image = new Bitmap(formFile.OpenReadStream());
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                    break;
                case "image/png":
                    image = new Bitmap(formFile.OpenReadStream());
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                    break;
                case "image/bmp":
                    image = new Bitmap(formFile.OpenReadStream());
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        return ms.ToArray();
                    }
                    break;
            }
            
            return null; 
        }
    }
}
