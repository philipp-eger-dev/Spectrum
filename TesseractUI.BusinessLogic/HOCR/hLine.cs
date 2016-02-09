using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Clock.Hocr
{

    /// <summary>
    /// Represents one line of text in a paragraph.
    /// </summary>
    public class hLine : HOcrClass
    {
        public IList<hLine> LinesInSameSentence { get; set; }
        public IList<hWord> Words { get; set; }
      
        public hLine()
        {
            Words = new List<hWord>();
            LinesInSameSentence = new List<hLine>();
       
        }

        public bool LineWasCombined { get; private set; }

        public hLine CombineLinesInSentence()
        {
            if (LinesInSameSentence == null || LinesInSameSentence.Count == 0)
                return this;
            hLine l = new hLine();
            l.Id = LinesInSameSentence.OrderBy(x => x.BBox.Left).First().Id;
            l.BBox = new Hocr.BBox();
            l.BBox.Top = LinesInSameSentence.Select(x => x.BBox.Top).Min();
            l.BBox.Height = LinesInSameSentence.Last().BBox.Height;
            l.BBox.Left = LinesInSameSentence.Select(x => x.BBox.Left).Min();
            l.BBox.Width = LinesInSameSentence.Select(x => x.BBox.Width).Sum();

            foreach (var o in LinesInSameSentence.OrderBy(x => x.BBox.Left))
                l.Text += o.Text;

            if (LinesInSameSentence.Count > 1)
                l.LineWasCombined = true;
            return l;

        }

        public void AlignTops()
        {
            hWord first = null;
            if (Words.Count == 0)
                return;

            float maxHeight = Words.OrderByDescending(x => x.BBox.Height).Take(1).Single().BBox.Height;
            float maxWidth = Words.Select(x => x.BBox.Width).Max();
            float top = Words.Select(x => x.BBox.Top).Min();

            foreach (hWord word in Words)
            {
                word.BBox.Top = top;
                word.BBox.Height = maxHeight;
            }
        }

    }
}
