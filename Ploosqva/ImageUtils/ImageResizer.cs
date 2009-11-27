using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ImageManipulation;
using Winista.Mime;

namespace Ploosqva.ImageUtils
{
    /// <summary>
    /// Class used to create high quality thumbnails of images
    /// </summary>
    public class ImageResizer
    {
        private static readonly string MIME_TYPES = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\XML\mime-types.xml");

        /// <summary>
        /// Resizes an image to the requested width. Does nothing if image is already
        /// narrower
        /// </summary>
        /// <param name="image">image to resize</param>
        /// <param name="width">max width</param>
        /// <returns>scaled image</returns>
        public static Bitmap Resize(Image image, int width)
        {
            if (image.Width < width)
                return new Bitmap(image);

            int w = width;
            int h = image.Height * w / image.Width;

            Bitmap thumb = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(thumb);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(image, new Rectangle(0, 0, w, h));

            g.Dispose();

            if (GetMime(image) == "image/gif")
            {
                OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                thumb = quantizer.Quantize(thumb);
            }

            return thumb;
        }

        /// <summary>
        /// Resizes an image to the requested dimensions
        /// </summary>
        /// <param name="image">image to resize</param>
        /// <param name="height">max height</param>
        /// <param name="width">max width</param>
        /// <returns>scaled image</returns>
        public static Bitmap Resize(Image image, int height, int width)
        {
            Int32 w, h;
            if (image.Width >= image.Height)
            {
                if (image.Width < width)
                    return new Bitmap(image);

                w = width;
                h = image.Height * w / image.Width;
            }
            else
            {
                if (image.Height < height)
                    return new Bitmap(image);

                h = height;
                w = image.Width * h / image.Height;

                if (w > width)
                {
                    w = width;
                    h = image.Height * w / image.Width;
                }
            }

            Bitmap thumb = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(thumb);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(image, new Rectangle(0, 0, w, h));

            g.Dispose();

            if (GetMime(image) == "image/gif")
            {
                OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                thumb = quantizer.Quantize(thumb);
            }

            return thumb;
        }

        /// <summary>
        /// Gets image's mime type
        /// </summary>
        private static string GetMime(Image image)
        {
            MimeTypes types = new MimeTypes(MIME_TYPES);

            MemoryStream stream = new MemoryStream();
            Bitmap img = new Bitmap(image);
            img.Save(stream, image.RawFormat);

            MimeType type = types.GetMimeType(SupportUtil.ToSByteArray(stream.ToArray()));

            stream.Dispose();
            img.Dispose();

            return type.Name;
        }
    }
}
