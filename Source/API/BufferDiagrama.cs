using Ada.Framework.UI.WinForms.Diagram.Entities;
using System.Collections.Generic;
using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.API
{
    public class BufferDiagrama
    {
        public enum Direccion { Arriba, Izquierda, Derecha, Abajo };

        public IList<Forma> Formas { get; set; }

        public BufferDiagrama()
        {
            Formas = new List<Forma>();
        }

        public Point ObtenerPosicionDisponible(Point punto, int margen, Direccion direccion, bool ultimaPosicion = false)
        {
            bool estaPuntoDisponible = true;

            do
            {
                estaPuntoDisponible = true;

                foreach (Forma xforma in Formas)
                {
                    if (xforma.Transparente) continue;

                    if (xforma.StartPoint.X == punto.X && xforma.StartPoint.Y == punto.Y)
                    {
                        punto = xforma.EndPoint;
                        estaPuntoDisponible = false;
                        break;
                    }
                }

                //Si el punto no esta disponible, lo desplaza según la dirección y el margen otorgado.
                if (!estaPuntoDisponible)
                {
                    if (direccion == Direccion.Abajo)
                    {
                        punto.Y += margen;
                    }
                    else if (direccion == Direccion.Arriba)
                    {
                        punto.Y -= margen;
                    }
                    else if (direccion == Direccion.Derecha)
                    {
                        punto.X += margen;
                    }
                    else if (direccion == Direccion.Izquierda)
                    {
                        punto.X -= margen;
                    }
                }
            }
            while (!estaPuntoDisponible);

            return punto;
        }

        public Point ObtenerPosicionDisponible(Point start, Point end, int margen, Direccion direccion, bool ultimaPosicion = false)
        {
            // Punto de inicio de la forma.
            Point puntoInicio = start;

            // Punto de fin de la forma para formar un cuadrado imaginario.
            Point puntoFin = end;

            bool estaPuntoDisponible = true;

            do
            {
                estaPuntoDisponible = true;

                foreach (Forma xforma in Formas)
                {
                    if (xforma.Transparente) continue;
                    
                    //Si el elemento esta dentro de los puntos del shape, entonces se debe probar correr el objeto.
                    if (EstaDentro(puntoInicio, xforma.StartPoint, xforma.EndPoint) || EstaDentro(puntoFin, xforma.StartPoint, xforma.EndPoint)
                        || EstaDentro(xforma.StartPoint, puntoInicio, puntoFin) || EstaDentro(xforma.EndPoint, puntoInicio, puntoFin))
                    {
                        if (direccion == Direccion.Abajo || direccion == Direccion.Arriba)
                        {
                            //puntoInicio.Y = xforma.EndPoint.Y;
                        }
                        else
                        {
                            //puntoInicio.X = xforma.EndPoint.X;
                        }
                        estaPuntoDisponible = false;
                        break;
                    }
                }

                //Si el punto no esta disponible, lo desplaza según la dirección y el margen otorgado.
                if (!estaPuntoDisponible)
                {
                    if (direccion == Direccion.Abajo)
                    {
                        puntoInicio.Y += margen;
                        puntoFin.Y += margen;
                    }
                    else if (direccion == Direccion.Arriba)
                    {
                        puntoInicio.Y -= margen;
                        puntoFin.Y -= margen;
                    }
                    else if (direccion == Direccion.Derecha)
                    {
                        puntoInicio.X += margen;
                        puntoFin.X += margen;
                    }
                    else if (direccion == Direccion.Izquierda)
                    {
                        puntoInicio.X -= margen;
                        puntoFin.X -= margen;
                    }
                }
            }
            while (!estaPuntoDisponible);

            return puntoInicio;
        }

        public Point ObtenerPosicionDisponible(Forma forma, int margen, Direccion direccion, bool ultimaPosicion = false)
        {
            return ObtenerPosicionDisponible(forma.StartPoint, forma.EndPoint, margen, direccion, ultimaPosicion);
        }

        public bool EstaDentro(Point punto, Point start, Point end)
        {
            return (punto.X >= start.X && punto.X <= end.X) && (punto.Y >= start.Y && punto.Y <= end.Y);
        }

        public void MoverObjetos(int margen, Direccion direccion)
        {
            foreach (Forma forma in Formas)
            {
                forma.StartPoint = ObtenerPosicionDisponible(forma, margen, direccion);
            }
        }
    }
}
