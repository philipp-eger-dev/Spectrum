using Clock.Hocr;
using System.Collections.Generic;
using TesseractUI.BusinessLogic.HOCR;

namespace TesseractUI.BusinessLogic
{
    public class hDocument : HOcrClass, IHOCRDocument
    {
        public IList<hPage> Pages { get; set; }

        public hDocument()
        {
            Pages = new List<hPage>();
        }

        public void AddFile(IParser parser, string hocr_file)
        {
            parser.ParseHOCR(this, hocr_file, true);
        } 
    }
}
