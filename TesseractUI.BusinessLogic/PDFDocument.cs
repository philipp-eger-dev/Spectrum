using Clock.Hocr;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.HOCR;

namespace TesseractUI.BusinessLogic
{
    public class PDFDocument
    {
        private string _FilePath;

        public PDFDocument(string filePath)
        {
            this._FilePath = filePath;
        }

        public void Ocr(string tesseractLanguageString)
        {
            IFileSystem fileSystem = new FileSystemAccess(this._FilePath);

            PdfReader pdf = new PdfReader(this._FilePath);

            List<string> pdfImages = GetPDFImages(
                pdf, this._FilePath, fileSystem.OutputDirectory);

            hDocument ocrDocument = new HOCRFileCreator().
                CreateHOCROfImage(fileSystem, pdfImages, tesseractLanguageString);

            AddOcrContent(fileSystem, pdf, ocrDocument, 300);
        }

        public void AddOcrContent(IFileSystem fileSystem, PdfReader r, hDocument ocrDocument, int Dpi, string FontName = null)
        {
            var mem = new FileStream(fileSystem.DestinationPDFPath, FileMode.Create, FileAccess.ReadWrite);
            PdfStamper pdfStamper = new PdfStamper(r, mem);

            int pageCounter = 1;
            foreach (hPage hrPage in ocrDocument.Pages)
            {
                PdfImportedPage page = pdfStamper.GetImportedPage(r, pageCounter);

                foreach (hParagraph para in hrPage.Paragraphs)
                {
                    foreach (hLine line in para.Lines)
                    {
                        line.AlignTops();

                        foreach (hWord c in line.Words)
                        {
                            c.CleanText();

                            BBox b = BBox.ConvertBBoxToPoints(c.BBox, Dpi);

                            if (b.Height > 50)
                                continue;
                            PdfContentByte cb = pdfStamper.GetUnderContent(pageCounter);

                            BaseFont base_font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false);

                            iTextSharp.text.Font font = new iTextSharp.text.Font(base_font);
                            if (FontName != null && FontName != string.Empty)
                            {
                                var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), FontName);
                                base_font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                // BaseFont base_font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false);
                                font = new iTextSharp.text.Font(base_font);
                            }

                            cb.BeginText();

                            float size = 0;// Math.Round(b.Height);
                            while (1 == 1 && size < 50)
                            {
                                var width = base_font.GetWidthPoint(c.Text, size);
                                if (width < b.Width)
                                {
                                    size += 1;
                                }
                                else
                                    break;
                            }
                            if (size < 10)
                                size = size - 1;

                            if (size == 0)
                                size = 1;

                            cb.SetFontAndSize(base_font, b.Height >= 2 ? (int)size : 2);
                            cb.SetTextMatrix(b.Left, page.Height - b.Top - b.Height);
                            cb.SetWordSpacing(PdfWriter.SPACE);

                            cb.ShowText(c.Text + " ");
                            cb.EndText();
                        }
                    }
                }

                pageCounter++;

                r.RemoveUnusedObjects();
            }


            pdfStamper.Close();
            pdfStamper.Reader.Close();
            mem.Close();

            mem = null;
            r = null;
        }

        private List<string> GetPDFImages(PdfReader pdf, string filePath, string outputPath)
        {
            List<string> pdfImages = new List<string>();

            for (int pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
            {
                pdfImages.Add(GetPageImage(pdf, filePath, pageNumber, outputPath));
            }

            return pdfImages;
        }

        private string GenerateOutputPath()
        {
            //TODO Output-Path noch dynamisch generieren lassen
            return @"D:\Test";
        }

        private string GetPageImage(
            PdfReader pdf, string filePath, int pageNumber, string outputPath)
        {
            RandomAccessFileOrArray randomAccess = new RandomAccessFileOrArray(filePath);

            string path = Path.Combine(outputPath, String.Format(@"{0}.jpg", pageNumber));

            try
            {
                PdfDictionary pdfDictionary = pdf.GetPageN(pageNumber);
                PdfDictionary res =
                  (PdfDictionary)PdfReader.GetPdfObject(pdfDictionary.Get(PdfName.RESOURCES));
                PdfDictionary xobj =
                  (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

                if (xobj != null)
                {
                    foreach (PdfName name in xobj.Keys)
                    {
                        PdfObject obj = xobj.Get(name);
                        if (obj.IsIndirect())
                        {
                            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                            PdfName type =
                              (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
                            if (PdfName.IMAGE.Equals(type))
                            {
                                int XrefIndex = Convert.ToInt32(((PRIndirectReference)obj).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                                PdfObject pdfObj = pdf.GetPdfObject(XrefIndex);
                                PdfStream pdfStrem = (PdfStream)pdfObj;
                                byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
                                if (bytes != null)
                                {
                                    using (MemoryStream memStream = new MemoryStream(bytes))
                                    {
                                        memStream.Position = 0;
                                        Image img = Image.FromStream(memStream);
                                        // must save the file while stream is open.
                                        if (!Directory.Exists(outputPath))
                                        {
                                            Directory.CreateDirectory(outputPath);
                                        }

                                        EncoderParameters parms = new EncoderParameters(1);
                                        parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
                                        ImageCodecInfo jpegEncoder = GetImageEncoder("JPEG");
                                        img.Save(path, jpegEncoder, parms);
                                        break;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return path;
        }

        public static ImageCodecInfo GetImageEncoder(string imageType)
        {
            imageType = imageType.ToUpperInvariant();

            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
            {
                if (info.FormatDescription == imageType)
                {
                    return info;
                }
            }

            return null;
        }
    }
}
