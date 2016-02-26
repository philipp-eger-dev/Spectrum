using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesseractUI.BusinessLogic.Tests
{
    [TestClass]
    public class PDFDocument_Tests
    {
        public void OCR_Test()
        {
            PDFDocument sourceDocument = new PDFDocument("");
            sourceDocument.Ocr(Tesseract.Language.English);
        }
    }
}
