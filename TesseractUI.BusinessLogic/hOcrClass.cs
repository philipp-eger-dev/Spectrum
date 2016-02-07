using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Clock.Hocr
{
    public class HOcrClass
    {
        public BBox BBox { get; set; }
        public string Id { get; set; }
        public string ClassName { get; set; }
        public string Text {get; set; 
        }
        public object Tag { get; set; }

        public override string ToString()
        {
            return string.Concat("Id: ", Id, "[", BBox.ToString(), "] Text: ", this.Text);
        }

        public void CleanText()
        {
            if (Text == null)
                return;

            string results = Text.Trim().Replace("&amp;", "&")
                                 .Replace("&lt;", "<")
                                 .Replace("&gt;", ">")
                                 .Replace("&quot;", "\"")
                                 .Replace("&#39;", "'")
                                 .Replace("&#44;", "-")
                                 .Replace("Ã¢â‚¬â€", "-")
                                 .Replace("â€","-")
                                 .Replace("\r\n", Environment.NewLine)
                                 .Replace("\n", Environment.NewLine);

            Text = results;
        }
    }
}
