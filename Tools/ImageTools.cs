namespace SkinCaApp.Tools
{
    public static class ImageTools
    {
        public static string? GetImageType(byte[] bytes)
        {
            
            if (bytes.Length < 8) return null;

           
            if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return "JPEG";
            }

            
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 &&
                bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
            {
                return "PNG";
            }

            
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38)
            {
                return "GIF";
            }

            
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return "BMP";
            }

            
            if ((bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00) ||
                (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A))
            {
                return "TIFF";
            }

            return null;
        }
       
    }
}

