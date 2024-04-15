using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    class UnMovable : GameObject
    {
        private Image img;
        public Image Img
        {
            get { return img; }
            set { img = value; Width = img.Width; Height = img.Height; }
        }

        public UnmovableType MyType { get; set; }

        protected override Pen GetPen()
        {
            return new Pen(Color.Green);
        }

        protected override Image GetImage()
        {
            return Img;
        }

        public UnMovable(int x, int y, Image img, UnmovableType type)
        {
            this.X = x;
            this.Y = y;
            this.Img = img;
            this.MyType = type;
        }
    }
}
