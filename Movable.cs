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
        Right,
    }
    class Movable : GameObject
    {
        private readonly object _lock = new object();

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
                    case Direction.Up: bmp = BitmapUp; break;
                    case Direction.Down: bmp = BitmapDown; break;
                    case Direction.Left: bmp = BitmapLeft; break;
                    case Direction.Right: bmp = BitmapRight; break;
                }
                // 事件处理线程中改变方向使用Bitmap资源. 本程序DrawSelf()中也要使用Bitmap资源,
                // 可能同时使用相同资源导致冲突.
                // 加锁解决冲突
                lock (_lock)
                {
                    Width = bmp.Width;
                    Height = bmp.Height;
                }
                
            }
                
        }

        public override void DrawSelf()
        {
            lock(_lock)
            {
                base.DrawSelf();
            }
        }

        protected void Move()
        {
            switch (Dir)
            {
                case Direction.Up: Y -= Speed; break;
                case Direction.Down: Y += Speed; break;
                case Direction.Left: X -= Speed; break;
                case Direction.Right: X += Speed; break;
            }
        }

        protected override Image GetImage()
        {
            Bitmap matchBitmap = null;
            switch (Dir)
            {
                case Direction.Up:
                    matchBitmap = BitmapUp;
                    break;
                case Direction.Down:
                    matchBitmap = BitmapDown;
                    break;
                case Direction.Left:
                    matchBitmap = BitmapLeft;
                    break;
                case Direction.Right:
                    matchBitmap = BitmapRight;
                    break;
                default:
                    matchBitmap = BitmapUp;
                    break;
            }

            // 黑色背景改为透明
            matchBitmap.MakeTransparent(Color.Black);
            
            return matchBitmap;
        }

    }
}
