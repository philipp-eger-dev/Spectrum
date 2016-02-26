using iTextSharp.text.pdf;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.FileSystem.Fakes;
using TesseractUI.BusinessLogic.Images;
using TesseractUI.BusinessLogic.PDF;
using TesseractUI.BusinessLogic.PDF.Fakes;

namespace TesseractUI.BusinessLogic.Tests
{
    [TestClass]
    public class PDFImageGenerator_Tests
    {
        [TestMethod]
        public void Test_GetPDFImages_FileNotExisting()
        {
            IFileSystem fileSystem = new StubIFileSystem()
            {
                ExistsString = (param) => { return false; }
            };
            PDFImageGenerator generator = new PDFImageGenerator();
            IPDFAccess pdf = new StubIPDFAccess()
            {

            };

            try
            {
                generator.GeneratePageImage(fileSystem, pdf, "", 1, "");

                Assert.IsTrue(false);
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
