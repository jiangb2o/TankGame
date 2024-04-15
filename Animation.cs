using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    enum AnimationType
    {
        Explosion,
    }

    class Animation : GameObject
    {
        private int playSpeed = 2;
        private int playCount = 0;

        private int index = 0;

        public bool PlayOver = false;

        private Bitmap[] bmpArray;

        // 传递碰撞位置, 动画中心在(x, y)
        public Animation(int x, int y, AnimationType animationType)
        {
            if(animationType == AnimationType.Explosion)
            {
                GetExplosionBmp();
            }

            foreach(Bitmap bmp in bmpArray)
            {
                bmp.MakeTransparent(Color.Black);
            }
            this.X = x - bmpArray[0].Width / 2;
            this.Y = y - bmpArray[0].Height / 2;
        }

        private void GetExplosionBmp()
        {
            bmpArray = new Bitmap[]
                {
                    Properties.Resources.EXP1,
                    Properties.Resources.EXP2,
                    Properties.Resources.EXP3,
                    Properties.Resources.EXP4,
                    Properties.Resources.EXP5
                };
        }

        protected override Pen GetPen()
        {
            return new Pen(Color.Red);
        }

        protected override Image GetImage()
        {
            return bmpArray[index];
        }

        public override void DrawSelf()
        {
            playCount++;
            // 播放帧索引
            index = (playCount - 1) / playSpeed;
            if (index >= bmpArray.Length)
            {
                PlayOver = true;
                return;
            }
            Width = bmpArray[index].Width;
            Height = bmpArray[index].Height;
            base.DrawSelf();
        }
    }
}
