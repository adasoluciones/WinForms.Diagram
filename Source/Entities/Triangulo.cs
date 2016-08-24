using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public class Triangulo : Forma
    {
        public override Point StartPoint
        {
            get
            {
                Point punto = new Point();
                punto.X = PuntoA.X;
                punto.Y = PuntoA.Y;

                if (punto.X > PuntoB.X) punto.X = PuntoB.X;
                if (punto.X > PuntoC.X) punto.X = PuntoC.X;

                if (punto.Y > PuntoB.Y) punto.Y = PuntoB.Y;
                if (punto.Y > PuntoC.Y) punto.Y = PuntoC.Y;

                return punto;
            }

            set
            {
                int diffX = PuntoA.X - StartPoint.X;
                int diffY = PuntoA.Y - StartPoint.Y;
                PuntoA = new Point() { X = value.X + diffX, Y = value.Y + diffY };

                diffX = PuntoB.X - StartPoint.X;
                diffY = PuntoB.Y - StartPoint.Y;
                PuntoB = new Point() { X = value.X + diffX, Y = value.Y + diffY };

                diffX = PuntoC.X - StartPoint.X;
                diffY = PuntoC.Y - StartPoint.Y;
                PuntoC = new Point() { X = value.X + diffX, Y = value.Y + diffY };
            }
        }
        public override Point EndPoint
        {
            get
            {
                Point punto = new Point();
                punto.X = PuntoA.X;
                punto.Y = PuntoA.Y;

                if (punto.X < PuntoB.X) punto.X = PuntoB.X;
                if (punto.X < PuntoC.X) punto.X = PuntoC.X;

                if (punto.Y < PuntoB.Y) punto.Y = PuntoB.Y;
                if (punto.Y < PuntoC.Y) punto.Y = PuntoC.Y;

                return punto;
            }

            set
            {
                int diffX = EndPoint.X - PuntoA.X;
                int diffY = EndPoint.Y - PuntoA.Y;
                PuntoA = new Point() { X = value.X - diffX, Y = value.Y - diffY };

                diffX = EndPoint.X - PuntoB.X;
                diffY = EndPoint.Y - PuntoB.Y;
                PuntoB = new Point() { X = value.X - diffX, Y = value.Y - diffY };

                diffX = EndPoint.X - PuntoC.X;
                diffY = EndPoint.Y - PuntoC.Y;
                PuntoC = new Point() { X = value.X - diffX, Y = value.Y - diffY };
            }
        }

        public Point PuntoA { get; set; }
        public Point PuntoB { get; set; }
        public Point PuntoC { get; set; }

        public Triangulo(Point puntoA, Point puntoB, Point puntoC) : base()
        {
            PuntoA = puntoA;
            PuntoB = puntoB;
            PuntoC = puntoC;
        }

        public Triangulo(int puntoAX, int puntoAY, int puntoBX, int puntoBY, int puntoCX, int puntoCY)
        {
            PuntoA = new Point()
            {
                X = puntoAX,
                Y = puntoAY
            };

            PuntoB = new Point()
            {
                X = puntoBX,
                Y = puntoBY
            };

            PuntoC = new Point()
            {
                X = puntoCX,
                Y = puntoCY
            };
        }

        public override void Dibujar(Graphics graphics)
        {
            Point[] puntos = new Point[3] { PuntoA, PuntoB, PuntoC };
            graphics.DrawPolygon(new Pen(ForeColor), puntos);
            graphics.FillPolygon(BackColor, puntos);
        }
    }
}
