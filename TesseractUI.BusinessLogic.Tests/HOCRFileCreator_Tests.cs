using Clock.Hocr;
using Clock.Hocr.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using TesseractUI.BusinessLogic.Exceptions;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.FileSystem.Fakes;
using TesseractUI.BusinessLogic.HOCR;
using TesseractUI.BusinessLogic.HOCR.Fakes;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.Tests
{
    [TestClass]
    public class HOCRFileCreator_Tests
    {
        [TestMethod]
        public void Test_CreateHOCROfImage_TesseractNotInstalled()
        {
            HOCRFileCreator fileCreator = new HOCRFileCreator();
            ITesseractProgram stubProgram = new StubITesseractProgram()
            {
                TesseractInstalledGet = () => { return false; }
            };

            try
            {
                fileCreator.CreateHOCROfImages(new hDocument(), new FileSystemAccess(""), stubProgram, new ProcessStarter(), new List<string>(), "de");
            }
            catch(TesseractNotInstalledException ex)
            {
                Assert.AreEqual(ex.Message, "Tesseract not installed");
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void Test_CreateHOCROfImage_FileNotExists()
        {
            HOCRFileCreator fileCreator = new HOCRFileCreator();
            ITesseractProgram stubProgram = new StubITesseractProgram()
            {
                TesseractInstalledGet = () => { return true; }
            };
            
            IFileSystem fileSystem = new StubIFileSystem()
            {
                ExistsString = (str) => { return false; }
            };

            try
            {
                fileCreator.CreateHOCROfImages(new hDocument(), fileSystem, stubProgram, new ProcessStarter(), new List<string>() { "NotThere" }, "de");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(true);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void Test_CreateHOCROfImage_Runs()
        {
            HOCRFileCreator fileCreator = new HOCRFileCreator();
            ITesseractProgram stubProgram = new StubITesseractProgram()
            {
                TesseractInstalledGet = () => { return true; },
                GenerateHOCROfImageProcessStarterStringString = (starter, path, language) => { return "Fake"; }
            };

            IFileSystem fileSystem = new StubIFileSystem()
            {
                ExistsString = (str) => { return true; }
            };

            IHOCRDocument document = new StubIHOCRDocument()
            {
                AddFileString = (str) => { }
            };

            try
            {
                fileCreator.CreateHOCROfImages(
                    new hDocument(), fileSystem, stubProgram, new ProcessStarter(), new List<string>() { "NotThere" }, "de");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(true);
                return;
            }

            Assert.Fail();
        }
    }
}
