using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesseractUI.BusinessLogic.Exceptions;
using TesseractUI.BusinessLogic.HOCR;
using TesseractUI.BusinessLogic.HOCR.Fakes;

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
                fileCreator.CreateHOCROfImage(stubProgram, new ProcessAccess.ProcessStarter(), null, null);
            }
            catch(TesseractNotInstalledException ex)
            {
                Assert.AreEqual(ex.Message, "Tesseract not installed");
                return;
            }

            Assert.Fail();
        }
    }
}
