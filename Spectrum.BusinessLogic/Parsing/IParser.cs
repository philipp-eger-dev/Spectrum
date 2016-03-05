using Clock.Hocr;

namespace TesseractUI.BusinessLogic
{
    public interface IParser
    {
        hDocument ParseHOCR(hDocument hOrcDoc, string hOcrFile, bool Append);
    }
}
