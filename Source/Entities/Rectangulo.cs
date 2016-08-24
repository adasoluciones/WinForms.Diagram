using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public class Rectangulo : Forma
    {
        public override Point StartPoint { get; set; }
        public override Point EndPoint
        {
            get
            {
                return new Point()
                {
                    X = StartPoint.X + (int)Width,
                    Y = StartPoint.Y + (int)Height
                };
            }

            set
            {
                Width = value.X - StartPoint.X;
                Height = value.Y - StartPoint.Y;
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        
        public Rectangulo(int X, int Y, int width, int height) : base()
        {
            StartPoint = new Point()
            {
                X = X,
                Y = Y
            };
            Width = width;
            Height = height;
        }

        public Rectangulo(Point punto, int width, int height) : base()
        {
            StartPoint = punto;
            Width = width;
            Height = height;
        }

        public override void Dibujar(Graphics graphics)
        {
            Rectangle rect = new Rectangle()
            {
                X = StartPoint.X,
                Y = StartPoint.Y,
                Width = Width,
                Height = Height
            };
            graphics.DrawRectangle(new Pen(ForeColor), rect);
            graphics.FillRectangle(BackColor, rect);
        }
    }
}
