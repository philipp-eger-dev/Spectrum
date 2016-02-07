using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Clock.Hocr
{
    public class hWord : HOcrClass
    {
        public IList<hChar> Characters { get; set; }
        public hWord()
        {

            Characters = new List<hChar>();
        }
		
        public void AlignCharacters()
        {
            if (Characters.Count == 0)
                return;

            float maxHeight = Characters.OrderByDescending(x=>x.BBox.Height).Take(1).Single().BBox.Height;
            float maxWidth = Characters.Select(x => x.BBox.Width).Max();
            float top = Characters.Select(x => x.BBox.Top).Min();
            foreach (hChar c in Characters)
            {
                c.BBox.Height = maxHeight;
                c.BBox.Top = top;
            }
        }
    }
}
