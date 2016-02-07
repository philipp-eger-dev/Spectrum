using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Clock.Hocr
{
    public class hPage : HOcrClass
    {

        public IList<hParagraph> Paragraphs { get; set; }

        public string ImageFile { get; set; }
        public int ImageFrameNumber { get; set; }
        public hPage()
        {
            Paragraphs = new List<hParagraph>();

        }

        public IList<HOcrClass> AllWordsOnPage
        {
            get
            {
                var res = new List<HOcrClass>();

                foreach (var para in Paragraphs)
                    foreach (var line in para.Lines)
                        foreach (var word in line.Words)
                            if(res.Where(x=>x.Id == word.Id).Count() == 0)
                                res.Add(word);

                return res.OrderBy(x => x.BBox.Top).ThenBy(x => x.BBox.Left).ToList<HOcrClass>() ;
            }
        }
        public int AverageWordCountPerLine { get; private set; }


        public IList<hLine> CombineSameRowLines()
        {
            IList<hLine> Lines = new List<hLine>();
            foreach (hParagraph p in Paragraphs)
            {
                foreach (var l in p.Lines)
                    if(Lines.Where(x=>x.Id == l.Id).Count() == 0)
                        Lines.Add(l);
            }
           
            IList<hLine> Results = new List<hLine>();

            var sortedLines = Lines.OrderBy(x => x.BBox.Top);
            foreach (var l in sortedLines)
            {
                l.CleanText();

                var LinesOnThisLine = 
                    Lines.Where(x=>Math.Abs(x.BBox.DefaultPointBBox.Top - l.BBox.DefaultPointBBox.Top) <= 3).OrderBy(x => x.BBox.Left).Distinct().ToList<hLine>();

                if(LinesOnThisLine.Select(x=>x.Id.Trim()).Distinct().Count() > 1)
                l.LinesInSameSentence = LinesOnThisLine;
           
                hLine c = l.CombineLinesInSentence();

                foreach (var liss in l.LinesInSameSentence)
                {
                    foreach (var w in liss.Words)
                        c.Words.Add(w);
                }
                if (Results.Where(x => x.Id == c.Id).Count() == 0)
                {
                  
                    Results.Add(c);
                }
             
            }

            if(Results.Count > 0)
            AverageWordCountPerLine = Convert.ToInt32(Math.Ceiling(Results.Select(x => x.Words.Count).Average()));
        //   return sortedLines.ToList<hLine>();
            return Results.OrderBy(x => x.BBox.Top).Distinct().ToList<hLine>();
        }

    }
}
