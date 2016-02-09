using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.Images;

namespace TesseractUI.BusinessLogic.Tests
{
    public class PDFImageGenerator_Tests
    {
        public void Test_GetPDFImages_FileNotExisting()
        {
            IFileSystem fileSystem = new FileSystemAccess();
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
