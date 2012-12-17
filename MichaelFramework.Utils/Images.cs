using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace MichaelFramework.Utils
{
    public class Images
    {
        private static int unifiedIconWidth = 25;
        public static int UnifiedIconWidth
        {
            get { return unifiedIconWidth; }
            set { unifiedIconWidth = value; }
        }

        public static Image ScaleImage(Image originalImage, int width)
        {
            if (originalImage == null) return null;
            return originalImage.GetThumbnailImage(width, width * originalImage.Height / originalImage.Width,
                null, IntPtr.Zero);
        }

        public static Image ScaleImage(string name, int width)
        {
            Image originalImage = GetImage(name);
            if (originalImage == null) return null;
            return originalImage.GetThumbnailImage(width, width * originalImage.Height / originalImage.Width,
                null, IntPtr.Zero);
        }

        public static Image ScaleIcon(Icon originalIcon, int width)
        {
            if (originalIcon == null) return null;
            return ScaleImage(originalIcon.ToBitmap(), width);
        }

        public static Image GetImage(string name)
        {
            string path = Path.Combine("Images", name);
            Image i = null;
            if (File.Exists(path + ".png"))
                i = Image.FromFile(path + ".png");
            else if (File.Exists(path + ".jpg"))
                i = Image.FromFile(path + ".jpg");
            else if (File.Exists(path + ".bmp"))
                i = Image.FromFile(path + ".bmp");
            else if (File.Exists(path + ".gif"))
                i = Image.FromFile(path + ".gif");
            else if (File.Exists(path + ".ico"))
                i = Image.FromFile(path + ".ico");
            return i;
        }

        public static Icon GetIcon(string name)
        {
            string path = Path.Combine("Images", name);
            if (File.Exists(path + ".ico"))
            {
                Icon i = Icon.ExtractAssociatedIcon(path + ".ico");
                return i;
            }
            return null;
        }
    }
}
