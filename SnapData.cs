using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapchatArchiver
{
    public class SnapData
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string DownloadUrl { get; set; }
    

        public static SnapData ParseSnapRow(HtmlNode trNode)
        {
            var metadata = new SnapData();
            var cells = trNode.SelectNodes("td");

            if (cells == null || cells.Count < 4) return null;

            string dateRaw = cells[0].InnerText.Replace(" UTC", "").Trim();
            if (DateTime.TryParse(dateRaw, out DateTime parsedDate))
            {
                metadata.Date = parsedDate.ToLocalTime();
            }

            metadata.Type = cells[1].InnerText.Trim();

            string gpsRaw = cells[2].InnerText;
            if (gpsRaw.Contains(":"))
            {
                var coords = gpsRaw.Split(':')[1].Split(',');
                if (coords.Length == 2)
                {
                    if (double.TryParse(coords[0].Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat))
                    {
                        metadata.Latitude = lat;
                    }

                    if (double.TryParse(coords[1].Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lon))
                    {
                        metadata.Longitude = lon;
                    }

                }
            }

            var anchor = cells[3].SelectSingleNode(".//a");
            if (anchor != null)
            {
                string onclick = anchor.GetAttributeValue("onclick", "");
                var match = System.Text.RegularExpressions.Regex.Match(onclick, @"'([^']*)'");
                if (match.Success)
                {
                    metadata.DownloadUrl = match.Groups[1].Value;
                }
            }

            return metadata;
        }

        public static void ProcessImageFinal(string imagePath, string overlayPath, SnapData meta)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            using (Bitmap baseImg = new Bitmap(ms))
            {
                MediaProcessor.MergeOverlay(baseImg, overlayPath);
                MediaProcessor.SetMeta(baseImg, meta);

                string tempSavePath = imagePath + ".tmp";
                baseImg.Save(tempSavePath, ImageFormat.Jpeg);

                baseImg.Dispose(); 
                ms.Dispose();

                File.Delete(imagePath);
                File.Move(tempSavePath, imagePath);
            }

            File.SetCreationTime(imagePath, meta.Date);
            File.SetLastWriteTime(imagePath, meta.Date);
        }

        public static void ProcessVideoFinal(string videoPath, SnapData meta)
        {
            MediaProcessor.SetVideoMetadata(videoPath, meta);
        }

    }

}
