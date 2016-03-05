using iTextSharp.text.pdf;
using System;
using System.IO;
using TesseractUI.BusinessLogic.FileSystem;

namespace TesseractUI.BusinessLogic.PDF
{
    public class ITextSharpPDFAccess : IPDFAccess
    {
        #region Fields
        private PdfReader _Reader;
        #endregion

        #region Properties
        public int NumberOfPages
        {
            get { return this._Reader.NumberOfPages; }
        }

        public PdfReader ReaderObject
        {
            get { return this._Reader; }
        }
        #endregion

        #region Constructor
        public ITextSharpPDFAccess(IFileSystem fileSystem, string filePath)
        {
            if (!fileSystem.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            this._Reader = new PdfReader(filePath);
        }
        #endregion

        #region Methods
        public PdfDictionary GetPageN(int pageNumber)
        {
            return this._Reader.GetPageN(pageNumber);
        }

        public PdfObject GetPdfObject(int xrefIndex)
        {
            return this._Reader.GetPdfObject(xrefIndex);
        }

        public void RemoveUnusedObjects()
        {
            this._Reader.RemoveUnusedObjects();
        }
        #endregion
    }
}
