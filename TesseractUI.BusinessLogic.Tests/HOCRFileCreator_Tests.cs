using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesseractUI.BusinessLogic.HOCR;

namespace TesseractUI.BusinessLogic.Tests
{
    [TestClass]
    public class HOCRFileCreator_Tests
    {
        [TestMethod]
        public void Test_CreateHOCROfImage_TesseractNotInstalled()
        {
            HOCRFileCreator fileCreator = new HOCRFileCreator();
        }
    }
}
