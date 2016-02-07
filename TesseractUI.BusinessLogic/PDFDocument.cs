using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TesseractUI.BusinessLogic
{
    public class PDFDocument
    {
        private string _FilePath;
        private string _OutputPath;

        public PDFDocument(string filePath)
        {
            this._FilePath = filePath;
            this._OutputPath = GenerateOutputPath();
        }

        public string Ocr(string tesseractLanguageString)
        {
            PdfReader pdf = new PdfReader(this._FilePath);

            List<string> pdfImages = new List<string>();

            for (int pageNumber = 1; pageNumber < pdf.NumberOfPages; pageNumber++)
            {
                pdfImages.Add(GetPageImage(pdf, this._FilePath, pageNumber, this._OutputPath));
            }

            return "";
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

            string path = System.IO.Path.Combine(outputPath, String.Format(@"{0}.jpg", pageNumber));

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

            return outputPath;
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
