using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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

            List<string> pdfImages = GetPDFImages(pdf, this._FilePath, this._OutputPath);

            CreateHOCROfImage(pdfImages, tesseractLanguageString);

            return "";
        }

        private void CreateHOCROfImage(List<string> pdfImagePaths, string tesseractLanguage)
        {
            foreach (string pdfImagePath in pdfImagePaths)
            {
                string outputFile = pdfImagePath.Replace(Path.GetExtension(pdfImagePath), ".hocr");

                string oArg = '"' + outputFile + '"';
                string commandArgs = 
                    String.Concat(pdfImagePath, " ", oArg, " -l " + tesseractLanguage + " -psm 1 hocr ");
                StartProcess(GetProgramPath("Tesseract-OCR", "tesseract.exe"), commandArgs);
            }
        }

        protected static string GetProgramPath(string ProgramDirectoryName, string ExeName)
        {
            //Check PATH first
            var enviromentPath = Environment.GetEnvironmentVariable("PATH");

            var paths = enviromentPath.Split(';');
            var exePath = paths.Select(x => Path.Combine(x, ExeName))
                               .Where(x => File.Exists(x))
                               .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(exePath) == false)
            {
                return exePath;
            }

            //Next check program files (64bit first)
            string ProgramW6432 = Environment.GetEnvironmentVariable("ProgramW6432");
            string pgFolder = Path.Combine(ProgramW6432 == null ? string.Empty : ProgramW6432, ProgramDirectoryName);
            string pg86Folder = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), ProgramDirectoryName);

            if (ProgramDirectoryName != null && ProgramDirectoryName != "")
            {
                if (Directory.Exists(pgFolder))
                {
                    string[] gsfiles = Directory.GetFiles(pgFolder, ExeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }

                if (Directory.Exists(pg86Folder))
                {
                    string[] gsfiles = Directory.GetFiles(pg86Folder, ExeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }
            }
            //Finally check directory of executing assembly
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, ExeName, SearchOption.AllDirectories);

            foreach (string gs in files)
                return gs;


            return null;

        }

        protected static void StartProcess(string FileToExecute, string Arguments)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = FileToExecute;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.Arguments = Arguments;
            p.StartInfo = info;

            try
            {
                p.Start();
                p.WaitForExit();
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
                throw x;
            }
        }

        private List<string> GetPDFImages(PdfReader pdf, string filePath, string outputPath)
        {
            List<string> pdfImages = new List<string>();

            for (int pageNumber = 1; pageNumber < pdf.NumberOfPages; pageNumber++)
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
