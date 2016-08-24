using System.Collections.Generic;
using System.Drawing;

namespace Ada.Framework.UI.WinForms.Diagram.Entities
{
    public abstract class Forma
    {
        public bool Transparente { get; set; }
        public abstract Point StartPoint { get; set; }
        public abstract Point EndPoint { get; set; }

        public Brush BackColor { get; set; }
        public Color ForeColor { get; set; }
        public object Data { get; set; }
        public IDictionary<string, object> KeyData { get; set; }

        public Forma()
        {
            ForeColor = Color.Black;
            BackColor = Brushes.Transparent;
            KeyData = new Dictionary<string, object>();
        }

        public abstract void Dibujar(Graphics graphics);
    }

    public abstract class Forma<T> : Forma
    {
        public T Padre { get; set; }

        public Forma(T padre) : base()
        {
            Padre = padre;
        }
    }
}
