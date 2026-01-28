using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SnapchatArchiver
{
    public static class MediaProcessor
    {
        public static void MergeOverlay(Bitmap baseImg, string overlayPath)
        {
            if (string.IsNullOrEmpty(overlayPath) || !File.Exists(overlayPath)) return;


            byte[] overlayBytes = File.ReadAllBytes(overlayPath); 
            using (MemoryStream ms = new MemoryStream(overlayBytes)) 
            using (Image overlay = Image.FromStream(ms)) 
            {
                using (Graphics g = Graphics.FromImage(baseImg))
                {
                    g.DrawImage(overlay, 0, 0, baseImg.Width, baseImg.Height);
                }
            }
        }

        public static void SetMeta(Bitmap img, SnapData meta)
        {

            PropertyItem prop = img.PropertyItems[0];
            prop.Id = 0x9003;
            prop.Type = 2;
            prop.Value = Encoding.ASCII.GetBytes(meta.Date.ToString("yyyy:MM:dd HH:mm:ss") + "\0");
            img.SetPropertyItem(prop);


            if (meta.Latitude.HasValue && meta.Longitude.HasValue)
            {

                prop.Id = 0x0001;
                prop.Value = Encoding.ASCII.GetBytes(meta.Latitude >= 0 ? "N\0" : "S\0");
                img.SetPropertyItem(prop);

                prop.Id = 0x0002; prop.Type = 5;
                prop.Value = ToRational(meta.Latitude.Value);
                img.SetPropertyItem(prop);

                prop.Id = 0x0003; prop.Type = 2;
                prop.Value = Encoding.ASCII.GetBytes(meta.Longitude >= 0 ? "E\0" : "W\0");
                img.SetPropertyItem(prop);

                prop.Id = 0x0004; prop.Type = 5;
                prop.Value = ToRational(meta.Longitude.Value);
                img.SetPropertyItem(prop);
            }
        }

        private static byte[] ToRational(double val)
        {
            val = Math.Abs(val);
            uint d = (uint)val;
            uint m = (uint)((val - d) * 60);
            uint s = (uint)((val - d - m / 60.0) * 3600 * 100);
            byte[] res = new byte[24];
            BitConverter.GetBytes(d).CopyTo(res, 0); BitConverter.GetBytes(1u).CopyTo(res, 4);
            BitConverter.GetBytes(m).CopyTo(res, 8); BitConverter.GetBytes(1u).CopyTo(res, 12);
            BitConverter.GetBytes(s).CopyTo(res, 16); BitConverter.GetBytes(100u).CopyTo(res, 20);
            return res;
        }


        public static void SetVideoMetadata(string filePath, SnapData meta)
        {
            try
            {
                var file = TagLib.File.Create(filePath);

                file.Tag.DateTagged = meta.Date;

                if (meta.Latitude.HasValue && meta.Longitude.HasValue)
                {
                    string latitude = meta.Latitude.Value.ToString("+00.0000;-00.0000", System.Globalization.CultureInfo.InvariantCulture);
                    string longitude = meta.Longitude.Value.ToString("+000.0000;-000.0000", System.Globalization.CultureInfo.InvariantCulture);
                    file.Tag.Comment = $"Location: {latitude}{longitude}/";
                }

                file.Save();


                File.SetCreationTime(filePath, meta.Date);
                File.SetLastWriteTime(filePath, meta.Date);
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }
        }
    }
}
