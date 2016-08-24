using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public class Poligono : Forma
    {
        public override Point EndPoint
        {
            get
            {
                Point punto = new Point();
                if (Puntos.Count > 0)
                {
                    punto.X = Puntos[0].X;
                    punto.Y = Puntos[0].Y;

                    for (int i = 1; i < Puntos.Count; i++)
                    {
                        if (punto.X > Puntos[i].X) punto.X = Puntos[i].X;
                        if (punto.X > Puntos[i].X) punto.X = Puntos[i].X;
                    }
                }
                return punto;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Point StartPoint
        {
            get
            {
                Point punto = new Point();
                if (Puntos.Count > 0)
                {
                    punto.X = Puntos[0].X;
                    punto.Y = Puntos[0].Y;

                    for (int i = 1; i < Puntos.Count; i++)
                    {
                        if (punto.X < Puntos[i].X) punto.X = Puntos[i].X;
                        if (punto.X < Puntos[i].X) punto.X = Puntos[i].X;
                    }
                }
                return punto;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual IList<Point> Puntos { get; set; }

        public Poligono()
        {
            Puntos = new List<Point>();
        }

        public override void Dibujar(Graphics graphics)
        {
            graphics.DrawPolygon(new Pen(ForeColor), Puntos.ToArray());
            graphics.FillPolygon(BackColor, Puntos.ToArray());
        }
    }
}
