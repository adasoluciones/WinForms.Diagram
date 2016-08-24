using System;
using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public class Texto : Forma
    {
        public override Point StartPoint { get; set; }
        public override Point EndPoint
        {
            get
            {
                int whidth = (int)(Text.Length * Font.Size);
                return new Point()
                {
                    X = StartPoint.X + whidth,
                    Y = StartPoint.Y + (int)Font.Size
                };
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        
        public string Text { get; set; }
        public Font Font { get; set; }
        public float Size { get { return Font.Size; } }
        public Brush Brush { get; set; }

        public Texto(Point punto, string texto, Font font, Brush brush)
        {
            StartPoint = punto;
            Text = texto;
            Font = font;
            Brush = brush;
        }

        public Texto(Point punto, string texto, Font font)
        {
            StartPoint = punto;
            Text = texto;
            Font = font;
            Brush = Brushes.Black;
        }

        public Texto(int X, int Y, string texto, Font font, Brush brush)
        {
            StartPoint = new Point()
            {
                X = X,
                Y = Y
            };
            Text = texto;
            Font = font;
            Brush = brush;
        }

        public Texto(int X, int Y, string texto, Font font)
        {
            StartPoint = new Point()
            {
                X = X,
                Y = Y
            };
            Text = texto;
            Font = font;
            Brush = Brushes.Black;
        }

        public Texto(Point punto, string texto, float size, Brush brush)
        {
            StartPoint = punto;
            Text = texto;
            Font = new Font(FontFamily.GenericSansSerif, size);
            Brush = brush;
        }

        public Texto(Point punto, string texto, float size)
        {
            StartPoint = punto;
            Text = texto;
            Font = new Font(FontFamily.GenericSansSerif, size);
            Brush = Brushes.Black;
        }

        public Texto(int X, int Y, string texto, float size, Brush brush)
        {
            StartPoint = new Point()
            {
                X = X,
                Y = Y
            };
            Text = texto;
            Font = new Font(FontFamily.GenericSansSerif, size);
            Brush = brush;
        }

        public Texto(int X, int Y, string texto, float size)
        {
            StartPoint = new Point()
            {
                X = X,
                Y = Y
            };
            Text = texto;
            Font = new Font(FontFamily.GenericSansSerif, size);
            Brush = Brushes.Black;
        }

        public override void Dibujar(Graphics graphics)
        {
            graphics.DrawString(Text, Font, Brush, StartPoint);
        }
    }
}
