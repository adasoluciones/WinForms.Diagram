using Ada.Framework.UI.WinForms.Diagram.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Ada.Framework.UI.WinForms.Diagram.API
{
    public class DiagramaSecuencia
    {
        private static BufferDiagrama Buffer { get; set; }
        private IDictionary<int, bool> TieneCuadroSecuencia;
        private List<Clase> clases = new List<Clase>();

        private int nivelSalto = 1;
        private int distanciaLlamadas = 30;

        public Point StartPoint
        {
            get
            {
                if (Buffer.Formas.Count == 0) return new Point(0, 0);

                int startX = Buffer.Formas.Min(c => c.StartPoint.X);
                int startY = Buffer.Formas.Min(c => c.StartPoint.Y);

                return new Point(startX, startY);
            }
        }

        public Point EndPoint
        {
            get
            {
                if (Buffer.Formas.Count == 0) return new Point(0, 0);

                int endX = Buffer.Formas.Max(c => c.EndPoint.X);
                int endY = Buffer.Formas.Max(c => c.EndPoint.Y);

                endX = (int)(endX * EscalaZoom);

                return new Point(endX, endY);
            }
        }

        public int Width
        {
            get
            {
                return EndPoint.X - StartPoint.X;
            }
        }

        public int Height
        {
            get
            {
                return EndPoint.Y - StartPoint.Y;
            }
        }

        public int XScrollMargin { get; set; }
        public int YScrollMargin { get; set; }

        public float EscalaZoom { get; set; }

        public event EventHandler AlHacerClickForma;

        public int WidthMargin { get; set; }
        public int HeightMargin { get; set; }

        public DiagramaSecuencia()
        {
            EscalaZoom = 1;
            Buffer = new BufferDiagrama();
            TieneCuadroSecuencia = new Dictionary<int, bool>();
        }

        public void Limpiar()
        {
            if (Buffer.Formas != null)
            {
                Buffer.Formas.Clear();
            }
        }

        private void ActualizarContexto(Linea linea, bool esRetorno)
        {
            int inicioX = esRetorno ? linea.EndPoint.X - 10 : linea.StartPoint.X;
            int finX = esRetorno ? linea.StartPoint.X : linea.EndPoint.X + 15;
            Brush colorRectangulo = new SolidBrush(Color.FromArgb(255, 211, 253));

            if (!TieneCuadroSecuencia[inicioX])
            {
                Rectangulo rect = new Rectangulo(
                         new Point(inicioX - 5, linea.StartPoint.Y), 10, 20)
                {
                    BackColor = colorRectangulo,
                    Transparente = true
                };
                Buffer.Formas.Add(rect);
                TieneCuadroSecuencia[inicioX] = true;
            }
            else
            {
                int indice = Buffer.Formas.IndexOf(Buffer.Formas.First(c => c is Rectangulo && c.StartPoint.X == linea.StartPoint.X - 5));
                Point puntoFinal = Buffer.Formas[indice].EndPoint;

                Buffer.Formas[indice].EndPoint = new Point(puntoFinal.X, linea.StartPoint.Y + 10);
                if (puntoFinal.Y > Buffer.Formas[indice].EndPoint.Y)
                {
                    Buffer.Formas[indice].EndPoint = puntoFinal;
                }
            }

            if (!TieneCuadroSecuencia[finX])
            {
                Rectangulo rectEnd = new Rectangulo(
                    new Point(linea.EndPoint.X + 10, linea.StartPoint.Y), 10, 20)
                {
                    BackColor = colorRectangulo,
                    Transparente = true
                };
                Buffer.Formas.Add(rectEnd);
                TieneCuadroSecuencia[finX] = true;
            }
            else
            {
                if (Buffer.Formas.Count(c => c is Rectangulo && c.EndPoint.X == linea.EndPoint.X - 5) > 0)
                {
                    int indice = Buffer.Formas.IndexOf(Buffer.Formas.First(c => c is Rectangulo && c.EndPoint.X == linea.EndPoint.X - 5));
                    Point puntoFinal = Buffer.Formas[indice].EndPoint;
                    Buffer.Formas[indice].EndPoint = new Point(puntoFinal.X, linea.StartPoint.Y + 10);

                    if (puntoFinal.Y > Buffer.Formas[indice].EndPoint.Y)
                    {
                        Buffer.Formas[indice].EndPoint = puntoFinal;
                    }
                }
            }
        }

        public Clase AgregarClase(string nombre, bool esUsuario = false)
        {
            Clase retorno = new Clase();

            retorno.Rectangulo = new Rectangulo(new Point() { X = 10, Y = 10 }, 100, 35) { BackColor = new SolidBrush(Color.FromArgb(176, 196, 222)), ForeColor = Color.FromArgb(176, 196, 222) };
            retorno.Rectangulo.StartPoint = Buffer.ObtenerPosicionDisponible(retorno.Rectangulo, 100, BufferDiagrama.Direccion.Derecha);

            if (esUsuario)
            {
                retorno.Rectangulo.BackColor = Brushes.Silver;
            }

            retorno.Texto = new Texto(new Point(retorno.Rectangulo.StartPoint.X + 25, retorno.Rectangulo.StartPoint.Y + 10), nombre, 10);
            retorno.Linea = new Linea(new Point()
            {
                X = retorno.Rectangulo.StartPoint.X + (retorno.Rectangulo.Width / 2),
                Y = retorno.Rectangulo.StartPoint.Y + retorno.Rectangulo.Height
            }, new Point()
            {
                X = retorno.Rectangulo.StartPoint.X + (retorno.Rectangulo.Width / 2),
                Y = 100
            }) { Transparente = true };

            Buffer.Formas.Add(retorno.Rectangulo);
            Buffer.Formas.Add(retorno.Texto);
            Buffer.Formas.Add(retorno.Linea);
            clases.Add(retorno);
            return retorno;
        }

        private void ActualizarUltimaLlamada(int ejeX, bool esInicio)
        {
            if (!TieneCuadroSecuencia.ContainsKey(ejeX))
            {
                TieneCuadroSecuencia.Add(ejeX, false);
            }
            else
            {
                TieneCuadroSecuencia[ejeX] = true;
            }
        }

        public void AgregarInicioLlamada(string nombreMetodo, Linea start, Linea end, string MethodGUID)
        {
            int tamañoTriangulo = 5;

            Linea linea = new Linea(
                new Point(start.StartPoint.X, start.StartPoint.Y),
                new Point(end.StartPoint.X - tamañoTriangulo - 10, end.StartPoint.Y)
            );
            linea.Data = MethodGUID;
            linea.StartPoint = Buffer.ObtenerPosicionDisponible(linea, distanciaLlamadas, BufferDiagrama.Direccion.Abajo, false);
            int ejeY = ((distanciaLlamadas * nivelSalto) + end.StartPoint.Y);

            linea.StartPoint = new Point(start.StartPoint.X, ejeY);

            Point puntoA = new Point(linea.EndPoint.X, linea.EndPoint.Y - tamañoTriangulo);
            Point puntoB = new Point(linea.EndPoint.X, linea.EndPoint.Y + tamañoTriangulo);
            Point puntoC = new Point(linea.EndPoint.X + (tamañoTriangulo * 2), linea.EndPoint.Y);

            ActualizarUltimaLlamada(linea.StartPoint.X, true);
            ActualizarUltimaLlamada(linea.EndPoint.X + 15, true);

            ActualizarContexto(linea, false);

            Buffer.Formas.Add(new Triangulo(puntoA, puntoB, puntoC) { BackColor = Brushes.Black });
            Buffer.Formas.Add(linea);
            Buffer.Formas.Add(new Texto(new Point(((linea.EndPoint.X - linea.StartPoint.X) / 2) + linea.StartPoint.X, linea.StartPoint.Y - 20), nombreMetodo, 10) { Data = nombreMetodo });
            nivelSalto++;
        }

        public void AgregarRetornoLlamada(Linea start, Linea end, bool lanzaExcepcion, bool isVoid, string MethodGUID)
        {
            int tamañoTriangulo = 5;

            Linea linea = new Linea(
                new Point(start.StartPoint.X, start.StartPoint.Y + distanciaLlamadas),
                new Point(end.StartPoint.X + tamañoTriangulo + 5, end.StartPoint.Y + distanciaLlamadas)
            );
            linea.Data = MethodGUID;
            linea.StartPoint = Buffer.ObtenerPosicionDisponible(linea, distanciaLlamadas, BufferDiagrama.Direccion.Abajo, false);
            linea.StartPoint = new Point(linea.StartPoint.X, linea.StartPoint.Y + (distanciaLlamadas * nivelSalto));

            int ejeY = ((distanciaLlamadas * nivelSalto) + end.StartPoint.Y);

            linea.StartPoint = new Point(start.StartPoint.X, ejeY);



            Color colorTriangulo = Color.Black;

            if (lanzaExcepcion)
            {
                linea.ForeColor = colorTriangulo = Color.Red;
            }

            Point puntoA = new Point(linea.EndPoint.X - tamañoTriangulo, linea.EndPoint.Y);
            Point puntoB = new Point(linea.EndPoint.X + tamañoTriangulo, linea.EndPoint.Y + tamañoTriangulo);
            Point puntoC = new Point(linea.EndPoint.X + tamañoTriangulo, linea.EndPoint.Y - tamañoTriangulo);

            ActualizarUltimaLlamada(linea.EndPoint.X - 10, false);

            ActualizarContexto(linea, true);

            if (!isVoid || lanzaExcepcion)
            {
                Buffer.Formas.Add(new Triangulo(puntoA, puntoB, puntoC) { BackColor = new SolidBrush(colorTriangulo), ForeColor = colorTriangulo });
                Buffer.Formas.Add(linea);
                nivelSalto++;
            }
        }

        public void Dibujar(Graphics graphics, Point puntoInicio)
        {
            foreach (Clase clase in clases)
            {
                clase.Linea.EndPoint = new Point(clase.Linea.EndPoint.X, Height + 11);
            }

            graphics.TranslateTransform(puntoInicio.X, puntoInicio.Y);

            foreach (Forma forma in Buffer.Formas)
            {
                forma.Dibujar(graphics);
            }

            Matrix mat = new Matrix();
            mat.Scale(EscalaZoom, EscalaZoom, MatrixOrder.Append);
            graphics.Transform = mat;
        }

        public void Dibujar(Graphics graphics)
        {
            Dibujar(graphics, new Point((int)graphics.VisibleClipBounds.X, (int)graphics.VisibleClipBounds.Y));
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (AlHacerClickForma != null) AlHacerClickForma(ObtenerForma(e.Location), e);
        }

        public Forma ObtenerForma(Point puntoMouse, Func<Forma, bool> filtro = null, int rangoError = 0)
        {
            int diferenciaLinea = 20;

            puntoMouse = new Point(puntoMouse.X - WidthMargin, puntoMouse.Y - HeightMargin);

            double ejeX = puntoMouse.X / (EscalaZoom);
            ejeX = Math.Round(ejeX);

            double ejeY = puntoMouse.Y / (EscalaZoom);
            ejeY = Math.Round(ejeY);

            puntoMouse = new Point((int)ejeX + XScrollMargin, (int)ejeY + YScrollMargin);

            foreach (Forma forma in Buffer.Formas)
            {
                if (filtro != null && !filtro(forma)) { continue; }

                int inicioX = forma.StartPoint.X - (rangoError / 2);
                int inicioY = forma.StartPoint.Y - (rangoError / 2);

                int finX = forma.EndPoint.X + (rangoError / 2);
                int finY = forma.EndPoint.Y + (rangoError / 2);

                //Si el punto de inicio es menor que el de fin. Entonces cambiamos el inicio por el fin y viceberza.
                if ((finX - inicioX) < (finY - inicioY))
                {
                    //Guardo InicioX.
                    int aux = inicioX;

                    inicioX = finX;
                    finX = aux;

                    //Guardo Inicio Y.
                    aux = inicioY;

                    inicioY = finY;
                    finY = aux;
                }

                if (forma is Linea)
                {
                    // ----------------
                    if ((finX - inicioX) > (finY - inicioY))
                    {
                        inicioY -= (diferenciaLinea / 2);
                        finY += (diferenciaLinea / 2);
                    }
                    else
                    {
                        inicioX -= diferenciaLinea;
                        finX += diferenciaLinea;
                    }
                }

                if (puntoMouse.X >= inicioX && puntoMouse.Y >= inicioY
                       && puntoMouse.X <= finX && puntoMouse.Y <= finY)
                {
                    return forma;
                }
            }
            return null;
        }
    }
}
