using Clock.Hocr;
using System.Collections.Generic;

namespace TesseractUI.BusinessLogic.HOCR
{
    public interface IHOCRDocument
    {
        IList<hPage> Pages { get; set; }

        void AddFile(string hocr_file);
    }
}
