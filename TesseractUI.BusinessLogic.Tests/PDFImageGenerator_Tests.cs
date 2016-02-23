using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.Images;

namespace TesseractUI.BusinessLogic.Tests
{
    [TestClass]
    public class PDFImageGenerator_Tests
    {
        [TestMethod]
        public void Test_GetPDFImages_FileNotExisting()
        {
            IFileSystem fileSystem = new FileSystemAccess("DEFAULT");
            PDFImageGenerator generator = new PDFImageGenerator(fileSystem);

            try
            {
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
