using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public class Linea : Forma
    {
        private Point startPoint;
        private Point endPoint;

        public override Point StartPoint
        {
            get
            {
                return startPoint;
            }

            set
            {
                int distanciaX = endPoint.X - startPoint.X;
                int distanciaY = endPoint.Y - startPoint.Y;
                endPoint = new Point(distanciaX + value.X, distanciaY + value.Y);
                startPoint = value;
            }
        }

        public override Point EndPoint 
        {
            get
            {
                return endPoint;
            }
            set
            {
                endPoint = value;
            }
        }

        public Linea(Point initialStartPoint, Point initialEndPoint) : base()
        {
            startPoint = initialStartPoint;
            endPoint = initialEndPoint;
        }

        public Linea(int startPointX, int startPointY, int endPointX, int endPointY) : base()
        {
            StartPoint = new Point()
            {
                X = startPointX,
                Y = startPointY
            };

            EndPoint = new Point()
            {
                X = endPointX,
                Y = endPointY
            };
        }

        public override void Dibujar(Graphics graphics)
        {
            graphics.DrawLine(new Pen(ForeColor), StartPoint, EndPoint);
        }
    }
}
