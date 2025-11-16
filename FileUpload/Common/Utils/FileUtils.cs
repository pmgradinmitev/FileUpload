namespace FileUpload.Common.Utils
{
    public static class FileUtils
    {
        public static string GetEmptyImagePath()
        {
            return "files" + Path.DirectorySeparatorChar + "common" + Path.DirectorySeparatorChar + "empty.jpg";
        }

        public static string GetPaintingImageDir()
        {
            return "files" + Path.DirectorySeparatorChar + "painting-images";
        }
    }
}
