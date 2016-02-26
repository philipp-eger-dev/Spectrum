using iTextSharp.text.pdf;

namespace TesseractUI.BusinessLogic.PDF
{
    public interface IPDFAccess
    {
        int NumberOfPages { get; }
        PdfReader ReaderObject { get; }

        PdfDictionary GetPageN(int pageNumber);
        PdfObject GetPdfObject(int xrefIndex);
        void RemoveUnusedObjects();
    }
}
