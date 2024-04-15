using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{

    abstract class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsDestroy = false;

        protected abstract Image GetImage();
        protected abstract Pen GetPen();

        public virtual void DrawSelf()
        {
            Graphics g = GameFramwork.g;
            if(GameFramwork.DrawMode == DrawMode.Normal)
            {
                g.DrawImage(GetImage(), X, Y);
            } else
            {
                g.DrawRectangle(GetPen(), X, Y, Width, Height);
            }
        }

        public virtual void Update()
        {
            DrawSelf();
        }

        public Rectangle GetRectangle()
        {
            Rectangle rectangle = new Rectangle(X, Y, Width, Height);
            return rectangle;
        }
    }
}
