using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Clock.Hocr
{
    public class hParagraph : HOcrClass
    {
        public IList<hLine> Lines { get; set; }
        // public IList<hWord> Words { get; set; }

        public hParagraph()
        {
            Lines = new List<hLine>();
         // Words = new List<hWord>();
            
        }
        
    }
}
