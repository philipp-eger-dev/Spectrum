using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Clock.Hocr
{
    public enum UnitFormat { Pixel, Point }
    public class BBox
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public UnitFormat Format { get; set; }
        public float CenterLine 
        {
            get
            {
                return Top + (Height / 2);
            }
        }
        public BBox() 
        {
            Format = UnitFormat.Pixel;
        }

        public BBox(string boxvalues)
        {
            string[] values = boxvalues.Trim().Split(new Char[char.MinValue]);
            if (values.Length < 4)
                return;

            int testout;

            if (Int32.TryParse(values[0].Trim(), out testout))
                Left = testout;

            if (Int32.TryParse(values[1].Trim(), out testout))
                Top = testout;

            if (Int32.TryParse(values[2].Trim(), out testout))
                Width = testout;

            if (Int32.TryParse(values[3].Trim(), out testout))
                Height = testout;


            //accurate width
            Width = Width - Left;
            //accurate height
            Height = Height - Top;

            Format = UnitFormat.Pixel;
        }

        public BBox DefaultPointBBox
        {
            get
            {
                return ConvertBBoxToPoints(this, 300);
            }
        }

        public static BBox ConvertBBoxToPoints(BBox bbox, int Resolution)
        {
           if (Resolution == 0)
            Resolution = 300;

            BBox newBbox = new BBox();
            newBbox.Left = (bbox.Left * 72) / Resolution;
            newBbox.Top = (bbox.Top * 72) / Resolution;
            newBbox.Width = (bbox.Width * 72) / Resolution;
            newBbox.Height = (bbox.Height * 72) / Resolution;
            newBbox.Format = UnitFormat.Point;
            return newBbox;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Left: " + Left.ToString());
            sb.AppendLine("Top: " + Top.ToString());
            sb.AppendLine("Width: " + Width.ToString());
            sb.AppendLine("Height: " + Height.ToString());
            sb.AppendLine("CenterLine: " + CenterLine.ToString());
            return sb.ToString();
        }


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(new Point((int)Left, (int)Top), new Size((int)Width, (int)Height));
            }
        }
    }
}
