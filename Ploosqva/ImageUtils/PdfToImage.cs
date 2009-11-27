using System;
using System.Drawing;
using System.IO;

namespace Ploosqva.ImageUtils
{
    ///<summary>
    /// Static class wich converts pdf pages to jpeg images
    ///</summary>
    public static class PdfToImage
    {
        /// <summary>
        /// Converts a single page from a pdf file to jpeg image
        /// </summary>
        /// <param name="pdfDocumentPath">path to pfd document</param>
        /// <param name="pageNum">0-based page number</param>
        /// <returns>converted page</returns>
        public static Image Convert(string pdfDocumentPath, int pageNum)
        {
            IntPtr instance = new IntPtr();
            string outputPath = Path.GetTempFileName();
            Image page = null;

            pageNum++;

            try
            {
                if (Gouda.Api.Ghostscript.NewInstance(out instance, IntPtr.Zero) == 0)
                {
                    string[] gsArgs =
                        {
                            "-dPARANOIDSAFER",
                            "-dBATCH",
                            "-dNOPAUSE",
                            "-dNOPROMPT",
                            "-dAlignToPixels=0",
                            "-sDEVICE=jpeg",
                            "-dFirstPage=" + pageNum,
                            "-dLastPage=" + pageNum,
                            "-dTextAlphaBits=4",
                            "-dGraphicsAlphaBits=4",
                            @"-sOutputFile=" + outputPath,
                            pdfDocumentPath
                        };

                    Gouda.Api.Ghostscript.InitWithArgs(instance, gsArgs.Length, gsArgs);
                    Gouda.Api.Ghostscript.Exit(instance);

                    page = Image.FromFile(outputPath);
                }
            }
            finally
            {
                Gouda.Api.Ghostscript.DeleteInstance(instance);
            }

            return page;
        }
    }
}