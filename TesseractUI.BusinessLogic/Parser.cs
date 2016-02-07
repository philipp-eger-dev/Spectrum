using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Clock.Hocr;
using HtmlAgilityPack;
using System.Xml.XPath;
using System.IO;
using System.Diagnostics;

namespace Clock.Hocr
{
    internal class Parser
    {
        static HtmlDocument doc;
        static string hOcrFilePath;
        static hDocument hDoc;
        static hPage currentPage;
        static hParagraph currentPara;
        static hLine currentLine;

        public static hDocument ParseHOCR(hDocument hOrcDoc, string hOcrFile, bool Append)
        {
  
            hDoc = hOrcDoc;

            if (doc == null)
                doc = new HtmlDocument();

            hOcrFilePath = hOcrFile; 
            if (File.Exists(hOcrFile) == false)
                throw new Exception("hocr file not found");

            currentPage = null;
            currentPara = null;
            currentLine = null;

            doc.Load(hOcrFile, Encoding.UTF8);

            HtmlNode body = doc.DocumentNode.SelectNodes("//body")[0];
            hDoc.ClassName = "body";
            HtmlNodeCollection nodes = body.SelectNodes("//div[@class='ocr_page']");
            ParseNodes(nodes);
            return hDoc;
        }

        private static void ParseNodes(HtmlNodeCollection nodes)
        {
            foreach (HtmlNode node in nodes)
            {
                if (node.HasAttributes)
                {
                    string className = string.Empty;
                    string title = string.Empty;
                    string id = string.Empty;

                    if (node.Attributes["class"] != null)
                        className = node.Attributes["class"].Value;
                    if (node.Attributes["title"] != null)
                        title = node.Attributes["title"].Value;
                    if (node.Attributes["Id"] != null)
                        id = node.Attributes["Id"].Value;

                    switch (className)
                    {
                        case "ocr_page":
                            currentPage = new hPage();
                            currentPage.ClassName = className;
                            currentPage.Id = id;
                            ParseTitle(title, currentPage);
                            currentPage.Text = node.InnerText;
                            hDoc.Pages.Add(currentPage);
                            break;
                        case "ocr_par":
                            currentPara = new hParagraph();
                            currentPara.ClassName = className;
                            currentPara.Id = id;
                            ParseTitle(title, currentPara);
                            currentPara.Text = node.InnerText;
                            currentPage.Paragraphs.Add(currentPara);
                            break;

                        case "ocr_line":
                            currentLine = new hLine();
                            currentLine.ClassName = className;
                            currentLine.Id = id;
                            ParseTitle(title, currentLine);
                            currentLine.Text = node.InnerText;
                            if (currentPage == null)
                            {
                                currentPage = new hPage();
                            }
                            if (currentPara == null)
                            {
                                currentPara = new hParagraph();
                                currentPage.Paragraphs.Add(currentPara);
                            }
                            currentPara.Lines.Add(currentLine);
                            break;

                        case "ocrx_word":
                            hWord w = new hWord();
                            w.ClassName = className;
                            w.Id = id;
                            ParseTitle(title, w);
                            w.Text = node.InnerText;
                            currentLine.Words.Add(w);
                            break;
					    case "ocr_word":
                            hWord w1 = new hWord();
                            w1.ClassName = className;
                            w1.Id = id;
                            ParseTitle(title, w1);
                            w1.Text = node.InnerText;
                            currentLine.Words.Add(w1);
                            break;
                        case "ocr_cinfo": //cuneiform only
                            ParseCharactersForLine(title);
                            break;

                    }

                }
                ParseNodes(node.ChildNodes);
            }
            
        }

        private static void ParseTitle(string Title, HOcrClass ocrclass)
        {
            if (Title == null)
                return;

            string[] values = Title.Split(new Char[] { ';' });
            foreach (string s in values)
            {
                if (s.Contains("image ") || s.Contains("file "))
                {
                    string filePath = s.Replace("image ", string.Empty).Replace("file ", string.Empty).Replace('"', ' ').Trim();

                    if (File.Exists(filePath))
                    {
                        if(ocrclass is hPage)
                             currentPage.ImageFile = filePath;
                    }
                    else
                    {
                        filePath = hOcrFilePath.Replace(Path.GetFileName(hOcrFilePath), Path.GetFileName(filePath));
                        {
                            if (ocrclass is hPage)
                                currentPage.ImageFile = filePath;
                        }
                    }
                }
                if (s.Contains("ppageno"))
                {
                    int frame;
                    if(Int32.TryParse(s.Replace("ppageno",""), out frame))
                         currentPage.ImageFrameNumber =frame;
                }
                if (s.Contains("bbox"))
                {
                    string coords = s.Replace("bbox", "");
                    BBox box = new BBox(coords);
                    ocrclass.BBox = box;

                }
            }
        }

        private static void ParseCharactersForLine(string Title)
        {
            if (Title == null)
                return;

            Title = Title.Replace("x_bboxes", "");

            string[] coords = Title.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string bbox = "";
            string word = "";
            hWord w = new hWord();
            int charPos = 0;

            for (int i = 0; i < coords.Length; i++)
            {
                if (i % 4 == 0 && i != 0)
                {
                    hChar c = new hChar();
                    BBox b = new BBox(bbox);
                    c.BBox = b;

                    char[] chars = currentLine.Text.ToCharArray();
                    c.Text = chars[charPos].ToString();

                    if (c.Text != " ")
                    {
                        word += c.Text;
                        if (w.BBox == null)
                        {
                            w.BBox = new BBox();
                            {
                                w.BBox.Height = c.BBox.Height;
                                w.BBox.Left = c.BBox.Left;
                                w.BBox.Top = currentLine.BBox.Top;
                            }
                        }
                    }
                    else
                    {
                        if (w.Characters.Count > 0)
                        {
                            hChar previouschar = w.Characters.OrderBy(x=>x.ListOrder).Last();
                            w.BBox.Width = (previouschar.BBox.Left + previouschar.BBox.Width) - w.BBox.Left;
                            w.Text = word + " ";
                            w.BBox.Height = w.Characters.Select(x => x.BBox.Height).Max();
                            w.CleanText();
                            w.CleanText();
                            if (w.Characters.Count > 0 && w.Text != null && w.Text.Trim() != "")
                                currentLine.Words.Add(w);
                            w = new hWord();
                            word = string.Empty;
                        }
                    }
              
                    bbox = string.Empty;
                    if (c.BBox.Left != -1)
                    {
                        c.ListOrder = charPos;
                        w.Characters.Add(c);
                    }
                    charPos += 1;
                }

                bbox += coords[i] + " ";
            }
            if (w.Characters.Count > 0 && word != null && word.Trim() != string.Empty)
            {
                w.Text = word;
                w.CleanText();
                currentLine.Words.Add(w);
            }

        }

    }
}
