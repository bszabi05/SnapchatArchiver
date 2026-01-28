using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapchatArchiver
{
    public static class ZipHandler
    {
        public static void HandleZipMedia(string zipFile, string baseName, string outputPath)
        {
            string subExtract = Path.Combine(outputPath, "temp_sub");
            if (Directory.Exists(subExtract))
            {
                Directory.Delete(subExtract, true);
            }
            ZipFile.ExtractToDirectory(zipFile, subExtract);

            foreach (var file in Directory.GetFiles(subExtract, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(file).ToLower();
                string finalName = (ext == ".png") ? $"{baseName}_overlay.png" : $"{baseName}{ext}";

                string targetPath = Path.Combine(outputPath, finalName);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
                File.Move(file, targetPath);
            }
            Directory.Delete(subExtract, true);
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
               
        }

        public static void ExtractMainArchive(string zipPath, string targetPath)
        {
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
            }
               

            ZipFile.ExtractToDirectory(zipPath, targetPath);
        }
    }
}
