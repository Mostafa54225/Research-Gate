using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace ResearchGate.Utils
{
    public class Helper
    {

        public static byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}