using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Ploosqva.ClassUtils.String;

namespace Ploosqva.ImageUtils
{
    /// <summary>
    /// Summary description for CaptchaImage.
    /// </summary>
    public class CaptchaImage
    {
        // Public properties (all read-only).
        private string familyName;
        private int height;
        private Bitmap image;

        // For generating random numbers.
        private readonly Random random = new Random();
        private readonly string text;
        private int width;

        /// <summary>
        /// Initializes new instance of CaptchaImage, and generates a random text wih
        /// the length specified
        /// </summary>
        public CaptchaImage(int textLength, int width, int height)
        {
            text = new RandomString(textLength).Value;
            SetDimensions(width, height);
            GenerateImage();
        }

        ///<summary>
        /// Initializes a new instance of the CaptchaImage class using the
        /// specified text, width and height.
        ///</summary>
        public CaptchaImage(string s, int width, int height)
        {
            text = s;
            SetDimensions(width, height);
            GenerateImage();
        }

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width, height and font family.
        // ====================================================================
        public CaptchaImage(string s, int width, int height, string familyName)
        {
            text = s;
            SetDimensions(width, height);
            SetFamilyName(familyName);
            GenerateImage();
        }

        public string Text
        {
            get { return text; }
        }

        public Bitmap Image
        {
            get { return image; }
        }

        public Int32 TextLength { get; set; }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~CaptchaImage()
        {
            Dispose(false);
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        // ====================================================================
        // Custom Dispose method to clean up unmanaged resources.
        // ====================================================================
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                image.Dispose();
        }

        // ====================================================================
        // Sets the image width and height.
        // ====================================================================
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", width,
                                                      "Argument out of range, must be greater than zero.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", height,
                                                      "Argument out of range, must be greater than zero.");
            this.width = width;
            this.height = height;
        }

        // ====================================================================
        // Sets the font used for the image text.
        // ====================================================================
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                Font font = new Font(this.familyName, 12F);
                this.familyName = familyName;
                font.Dispose();
            }
            catch (Exception ex)
            {
                this.familyName = FontFamily.GenericSerif.Name;
            }
        }

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            const float v = 4F;
            PointF[] points =
                {
                    new PointF(random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                    new PointF(rect.Width - random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                    new PointF(random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v),
                    new PointF(rect.Width - random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v)
                };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            image = bitmap;
        }
    }
}