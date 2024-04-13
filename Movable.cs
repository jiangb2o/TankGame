using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    class Movable : GameObject
    {
        public int WindowWidth { get; set; }
        public int WindowHeigth { get; set; }

        public Bitmap BitmapUp { get; set; }
        public Bitmap BitmapDown { get; set; }
        public Bitmap BitmapLeft { get; set; }
        public Bitmap BitmapRight { get; set; }

        public int Speed { get; set; }

        private Direction dir;
        public Direction Dir { get { return dir; }
            set
            {
                dir = value;
                Bitmap bmp = null;
                switch (dir)
                {
                    case Direction.Up: bmp = (Bitmap)BitmapUp.Clone(); break;
                    case Direction.Down : bmp = (Bitmap)BitmapDown.Clone(); break;
                    case Direction.Right : bmp = (Bitmap)BitmapRight.Clone(); break;
                    case Direction.Left: bmp = (Bitmap)BitmapLeft.Clone(); break;
                }
                Width = bmp.Width;
                Height = bmp.Height;
            }
                
        }
        

        protected override Image GetImage()
        {
            Bitmap matchBitmap = null;
            switch (Dir)
            {
                // 使用bitmap副本, 防止Bitmap同时被多个方法调用 或跨进程使用
                case Direction.Up:
                    matchBitmap = (Bitmap)BitmapUp.Clone();
                    break;
                case Direction.Down:
                    matchBitmap = (Bitmap)BitmapDown.Clone();
                    break;
                case Direction.Left:
                    matchBitmap = (Bitmap)BitmapLeft.Clone();
                    break;
                case Direction.Right:
                    matchBitmap = (Bitmap)BitmapRight.Clone();
                    break;
                default:
                    matchBitmap = (Bitmap)BitmapUp.Clone();
                    break;
            }
            // 黑色背景改为透明
            matchBitmap.MakeTransparent(Color.Black);
            return matchBitmap;
        }
    }
}
